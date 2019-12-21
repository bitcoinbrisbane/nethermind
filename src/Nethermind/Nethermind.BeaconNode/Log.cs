﻿//  Copyright (c) 2018 Demerzel Solutions Limited
//  This file is part of the Nethermind library.
// 
//  The Nethermind library is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  The Nethermind library is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU Lesser General Public License for more details.
// 
//  You should have received a copy of the GNU Lesser General Public License
//  along with the Nethermind. If not, see <http://www.gnu.org/licenses/>.

using System;
using Microsoft.Extensions.Logging;
using Nethermind.Core2.Crypto;
using Nethermind.Core2.Types;
using Nethermind.Core2.Containers;
using Attestation = Nethermind.BeaconNode.Containers.Attestation;
using AttesterSlashing = Nethermind.BeaconNode.Containers.AttesterSlashing;
using BeaconBlock = Nethermind.BeaconNode.Containers.BeaconBlock;
using BeaconBlockBody = Nethermind.BeaconNode.Containers.BeaconBlockBody;
using BeaconState = Nethermind.BeaconNode.Containers.BeaconState;
using Deposit = Nethermind.BeaconNode.Containers.Deposit;
using ProposerSlashing = Nethermind.BeaconNode.Containers.ProposerSlashing;

namespace Nethermind.BeaconNode
{
    internal static class Log
    { 
        // 1bxx preliminary

        // 2bxx completion
        public static readonly Action<ILogger, Deposit, BeaconState, Exception?> ProcessDeposit =
            LoggerMessage.Define<Deposit, BeaconState>(LogLevel.Information,
                new EventId(2000, nameof(ProcessDeposit)),
                "Process block operation deposit {Deposit} for state {BeaconState}.");

        public static readonly Action<ILogger, Slot, BeaconState, Exception?> ProcessSlots =
            LoggerMessage.Define<Slot, BeaconState>(LogLevel.Information,
                new EventId(2001, nameof(ProcessSlots)),
                "Process slots to {Slot} for state {BeaconState}");
        
        public static readonly Action<ILogger, Slot, BeaconState, Exception?> ProcessSlot =
            LoggerMessage.Define<Slot, BeaconState>(LogLevel.Information,
                new EventId(2002, nameof(ProcessSlot)),
                "Process current slot {Slot} for state {BeaconState}");
        
        public static readonly Action<ILogger, BeaconState, Exception?> ProcessJustificationAndFinalization =
            LoggerMessage.Define<BeaconState>(LogLevel.Information,
                new EventId(2003, nameof(ProcessJustificationAndFinalization)),
                "Process epoch justification and finalization state {BeaconState}");

        public static readonly Action<ILogger,  BeaconState, Exception?> ProcessEpoch =
            LoggerMessage.Define<BeaconState>(LogLevel.Information,
                new EventId(2004, nameof(ProcessEpoch)),
                "Process end of epoch for state {BeaconState}");

        public static readonly Action<ILogger, bool, BeaconBlock, BeaconState, Exception?> ProcessBlock =
            LoggerMessage.Define<bool, BeaconBlock, BeaconState>(LogLevel.Information,
                new EventId(2005, nameof(ProcessBlock)),
                "Process (validate {Validate}) block {BeaconBlock} for state {BeaconState}");
        
        public static readonly Action<ILogger, BeaconBlock,  Exception?> ProcessBlockHeader =
            LoggerMessage.Define<BeaconBlock>(LogLevel.Information,
                new EventId(2006, nameof(ProcessBlockHeader)),
                "Process block header for block {BeaconBlock}");

        public static readonly Action<ILogger,  BeaconBlockBody, Exception?> ProcessRandao =
            LoggerMessage.Define<BeaconBlockBody>(LogLevel.Information,
                new EventId(2007, nameof(ProcessRandao)),
                "Process block randao for block body {BeaconBlockBody}");

        public static readonly Action<ILogger,  BeaconBlockBody, Exception?> ProcessEth1Data =
            LoggerMessage.Define<BeaconBlockBody>(LogLevel.Information,
                new EventId(2008, nameof(ProcessEth1Data)),
                "Process block ETH1 data for block body {BeaconBlockBody}");

        public static readonly Action<ILogger,  BeaconBlockBody, Exception?> ProcessOperations =
            LoggerMessage.Define<BeaconBlockBody>(LogLevel.Information,
                new EventId(2009, nameof(ProcessOperations)),
                "Process block operations for block body {BeaconBlockBody}");
        
        public static readonly Action<ILogger, ProposerSlashing, Exception?> ProcessProposerSlashing =
            LoggerMessage.Define<ProposerSlashing>(LogLevel.Information,
                new EventId(2010, nameof(ProcessProposerSlashing)),
                "Process block operation proposer slashing {ProposerSlashing}");
        
        public static readonly Action<ILogger, AttesterSlashing, Exception?> ProcessAttesterSlashing =
            LoggerMessage.Define<AttesterSlashing>(LogLevel.Information,
                new EventId(2011, nameof(ProcessAttesterSlashing)),
                "Process block operation attester slashing {AttesterSlashing}");
        
        public static readonly Action<ILogger, VoluntaryExit, BeaconState, Exception?> ProcessVoluntaryExit =
            LoggerMessage.Define<VoluntaryExit, BeaconState>(LogLevel.Information,
                new EventId(2013, nameof(ProcessVoluntaryExit)),
                "Process block operation voluntary exit {VoluntaryExit} for state {BeaconState}.");
        
        public static readonly Action<ILogger, Attestation, BeaconState, Exception?> ProcessAttestation =
            LoggerMessage.Define<Attestation, BeaconState>(LogLevel.Information,
                new EventId(2012, nameof(ProcessAttestation)),
                "Process block operation attestation {Attestation} for state {BeaconState}.");
        
        public static readonly Action<ILogger,  BeaconState, Exception?> ProcessRewardsAndPenalties =
            LoggerMessage.Define<BeaconState>(LogLevel.Information,
                new EventId(2014, nameof(ProcessRewardsAndPenalties)),
                "Process epoch rewards and penalties state {BeaconState}");

        public static readonly Action<ILogger,  BeaconState, Exception?> ProcessFinalUpdates =
            LoggerMessage.Define<BeaconState>(LogLevel.Information,
                new EventId(2015, nameof(ProcessFinalUpdates)),
                "Process epoch final updates state {BeaconState}");

        public static readonly Action<ILogger,  BeaconState, Exception?> ProcessRegistryUpdates =
            LoggerMessage.Define<BeaconState>(LogLevel.Information,
                new EventId(2016, nameof(ProcessRegistryUpdates)),
                "Process epoch registry updates state {BeaconState}");

        public static readonly Action<ILogger,  BeaconState, Exception?> ProcessSlashings =
            LoggerMessage.Define<BeaconState>(LogLevel.Information,
                new EventId(2017, nameof(ProcessSlashings)),
                "Process epoch slashings state {BeaconState}");
        
        public static readonly Action<ILogger, bool, BeaconState, BeaconBlock, Exception?> StateTransition =
            LoggerMessage.Define<bool, BeaconState, BeaconBlock>(LogLevel.Information,
                new EventId(2020, nameof(StateTransition)),
                "State transition (validate {Validate}) for state {BeaconState} with block {BeaconBlock}");

        // 3bxx debug
        public static readonly Action<ILogger, ValidatorIndex, string, Gwei, Exception?> RewardForValidator =
            LoggerMessage.Define<ValidatorIndex, string, Gwei>(LogLevel.Debug,
                new EventId(3001, nameof(RewardForValidator)),
                "Reward for validator {ValidatorIndex}: {RewardName} +{Reward}.");

        public static readonly Action<ILogger, ValidatorIndex, string, Gwei, Exception?> PenaltyForValidator =
            LoggerMessage.Define<ValidatorIndex, string, Gwei>(LogLevel.Debug,
                new EventId(3002, nameof(PenaltyForValidator)),
                "Penalty for validator {ValidatorIndex}: {PenaltyName} -{Penalty}.");


        public static readonly Action<ILogger, Slot, BlsSignature, Slot, Exception?> NewBlockSkippedSlots =
            LoggerMessage.Define<Slot, BlsSignature, Slot>(LogLevel.Debug,
                new EventId(3200, nameof(NewBlockSkippedSlots)),
                "Request for new block for slot {Slot} for randao {RandaoReveal} is skipping from parent slot {ParentSlot}.");

        // 4bxx warning

        // 5bxx error

        // 8bxx finalization

        // 9bxx critical


    }
}