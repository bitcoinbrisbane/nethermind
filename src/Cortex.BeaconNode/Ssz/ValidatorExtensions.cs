﻿using System.Collections.Generic;
using Cortex.Containers;
using Cortex.SimpleSerialize;

namespace Cortex.BeaconNode.Ssz
{
    public static class ValidatorExtensions
    {
        public static SszContainer ToSszContainer(this Validator item)
        {
            return new SszContainer(GetValues(item));
        }

        private static IEnumerable<SszElement> GetValues(Validator item)
        {
            //pubkey: BLSPubkey
            yield return new SszBasicVector(item.PublicKey);
            //withdrawal_credentials: Hash  # Commitment to pubkey for withdrawals and transfers
            yield return new SszBasicVector(item.WithdrawalCredentials);
            //effective_balance: Gwei  # Balance at stake
            yield return new SszBasicElement(item.EffectiveBalance);
            //slashed: boolean
            //yield return new SszBasicElement(item.IsSlashed);

            //# Status epochs
            //activation_eligibility_epoch: Epoch  # When criteria for activation were met
            yield return new SszBasicElement(item.ActivationEligibilityEpoch);
            //activation_epoch: Epoch
            yield return new SszBasicElement(item.ActivationEpoch);
            //exit_epoch: Epoch
            yield return new SszBasicElement(item.ExitEpoch);
            //withdrawable_epoch: Epoch  # When validator can withdraw or transfer funds
            yield return new SszBasicElement(item.WithdrawableEpoch);
        }
    }
}
