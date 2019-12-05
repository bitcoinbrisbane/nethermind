﻿using System;
using Cortex.BeaconNode.Configuration;
using Cortex.BeaconNode.Storage;
using Cortex.BeaconNode.Ssz;
using Cortex.BeaconNode.Tests.Helpers;
using Cortex.Containers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Cortex.Containers.Json;

namespace Cortex.BeaconNode.Tests.Fork
{
    [TestClass]
    public class GetHeadTest
    {
        [TestMethod]
        public async Task GenesisHead()
        {
            // Arrange
            var testServiceProvider = TestSystem.BuildTestServiceProvider(useStore: true);
            var state = TestState.PrepareTestState(testServiceProvider);

            var miscellaneousParameters = testServiceProvider.GetService<IOptions<MiscellaneousParameters>>().Value;
            var timeParameters = testServiceProvider.GetService<IOptions<TimeParameters>>().Value;
            var stateListLengths = testServiceProvider.GetService<IOptions<StateListLengths>>().Value;
            var maxOperationsPerBlock = testServiceProvider.GetService<IOptions<MaxOperationsPerBlock>>().Value;

            var options = new System.Text.Json.JsonSerializerOptions { WriteIndented = true };
            options.AddCortexContainerConverters();
            var debugState = System.Text.Json.JsonSerializer.Serialize(state, options);
            
            // Initialization
            var forkChoice = testServiceProvider.GetService<ForkChoice>();
            var store = forkChoice.GetGenesisStore(state);

            // Act
            var headRoot = await forkChoice.GetHeadAsync(store);

            // Assert
            var stateRoot = state.HashTreeRoot(miscellaneousParameters, timeParameters, stateListLengths, maxOperationsPerBlock);
            var genesisBlock = new BeaconBlock(stateRoot);
            var expectedRoot = genesisBlock.SigningRoot(miscellaneousParameters, maxOperationsPerBlock);

            headRoot.ShouldBe(expectedRoot);
        }

        [TestMethod]
        public async Task ChainNoAttestations()
        {
            // Arrange
            var testServiceProvider = TestSystem.BuildTestServiceProvider(useStore: true);
            var state = TestState.PrepareTestState(testServiceProvider);

            var miscellaneousParameters = testServiceProvider.GetService<IOptions<MiscellaneousParameters>>().Value;
            var maxOperationsPerBlock = testServiceProvider.GetService<IOptions<MaxOperationsPerBlock>>().Value;

            // Initialization
            var forkChoice = testServiceProvider.GetService<ForkChoice>();
            var store = forkChoice.GetGenesisStore(state);

            // On receiving a block of `GENESIS_SLOT + 1` slot
            var block1 = TestBlock.BuildEmptyBlockForNextSlot(testServiceProvider, state, signed: true);
            TestState.StateTransitionAndSignBlock(testServiceProvider, state, block1);
            AddBlockToStore(testServiceProvider, store, block1);

            // On receiving a block of next epoch
            var block2 = TestBlock.BuildEmptyBlockForNextSlot(testServiceProvider, state, signed: true);
            TestState.StateTransitionAndSignBlock(testServiceProvider, state, block2);
            AddBlockToStore(testServiceProvider, store, block2);

            // Act
            var headRoot = await forkChoice.GetHeadAsync(store);

            // Assert
            var expectedRoot = block2.SigningRoot(miscellaneousParameters, maxOperationsPerBlock);
            headRoot.ShouldBe(expectedRoot);
        }


        [TestMethod]
        public async Task ShorterChainButHeavierWeight()
        {
            // Arrange
            var testServiceProvider = TestSystem.BuildTestServiceProvider(useStore: true);
            var state = TestState.PrepareTestState(testServiceProvider);

            var miscellaneousParameters = testServiceProvider.GetService<IOptions<MiscellaneousParameters>>().Value;
            var maxOperationsPerBlock = testServiceProvider.GetService<IOptions<MaxOperationsPerBlock>>().Value;

            // Initialization
            var forkChoice = testServiceProvider.GetService<ForkChoice>();
            var store = forkChoice.GetGenesisStore(state);
            var genesisState = BeaconState.Clone(state);

            // build longer tree
            var longState = BeaconState.Clone(genesisState);
            for (var i = 0; i < 3; i++)
            {
                var longBlock = TestBlock.BuildEmptyBlockForNextSlot(testServiceProvider, longState, signed: true);
                TestState.StateTransitionAndSignBlock(testServiceProvider, longState, longBlock);
                AddBlockToStore(testServiceProvider, store, longBlock);
            }

            // build short tree
            var shortState = BeaconState.Clone(genesisState);
            var shortBlock = TestBlock.BuildEmptyBlockForNextSlot(testServiceProvider, shortState, signed: true);
            shortBlock.Body.SetGraffiti(new Bytes32(Enumerable.Repeat((byte)0x42, 32).ToArray()));
            TestState.StateTransitionAndSignBlock(testServiceProvider, shortState, shortBlock);
            AddBlockToStore(testServiceProvider, store, shortBlock);

            var shortAttestation = TestAttestation.GetValidAttestation(testServiceProvider, shortState, shortBlock.Slot, CommitteeIndex.None, signed: true);
            AddAttestationToStore(testServiceProvider, store, shortAttestation);

            // Act
            var headRoot = await forkChoice.GetHeadAsync(store);

            // Assert
            var expectedRoot = shortBlock.SigningRoot(miscellaneousParameters, maxOperationsPerBlock);
            headRoot.ShouldBe(expectedRoot);
        }

        private void AddAttestationToStore(IServiceProvider testServiceProvider, IStore store, Attestation attestation)
        {
            var timeParameters = testServiceProvider.GetService<IOptions<TimeParameters>>().Value;
            var miscellaneousParameters = testServiceProvider.GetService<IOptions<MiscellaneousParameters>>().Value;
            var maxOperationsPerBlock = testServiceProvider.GetService<IOptions<MaxOperationsPerBlock>>().Value;
            var forkChoice = testServiceProvider.GetService<ForkChoice>();

            store.TryGetBlock(attestation.Data.BeaconBlockRoot, out var parentBlock);
            var parentSigningRoot = parentBlock.SigningRoot(miscellaneousParameters, maxOperationsPerBlock);
            store.TryGetBlockState(parentSigningRoot, out var preState);
            var blockTime = preState.GenesisTime + (ulong)parentBlock.Slot * timeParameters.SecondsPerSlot;
            var nextEpochTime = blockTime + (ulong)timeParameters.SlotsPerEpoch * timeParameters.SecondsPerSlot;

            if (store.Time < blockTime)
            {
                forkChoice.OnTick(store, blockTime);
            }

            forkChoice.OnAttestation(store, attestation);
        }

        private void AddBlockToStore(IServiceProvider testServiceProvider, IStore store, BeaconBlock block)
        {
            var timeParameters = testServiceProvider.GetService<IOptions<TimeParameters>>().Value;
            var forkChoice = testServiceProvider.GetService<ForkChoice>();

            store.TryGetBlockState(block.ParentRoot, out var preState);
            var blockTime = preState.GenesisTime + (ulong)block.Slot * timeParameters.SecondsPerSlot;

            if (store.Time < blockTime)
            {
                forkChoice.OnTick(store, blockTime);
            }

            forkChoice.OnBlock(store, block);
        }
    }
}
