﻿/*
 * Copyright (c) 2018 Demerzel Solutions Limited
 * This file is part of the Nethermind library.
 *
 * The Nethermind library is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * The Nethermind library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with the Nethermind. If not, see <http://www.gnu.org/licenses/>.
 */

using System.Numerics;
using Nethermind.Core.Crypto;

namespace Nethermind.Core.Test.Builders
{
    public class BlockBuilder : BuilderBase<Block>
    {
        public BlockBuilder()
        {
            BlockHeader header = Build.A.BlockHeader.TestObject;
            TestObjectInternal = new Block(header);
        }

        public BlockBuilder WithNumber(BigInteger number)
        {
            TestObjectInternal.Header.Number = number;
            return this;
        }
        
        public BlockBuilder WithTotalDifficulty(BigInteger difficulty)
        {
            TestObjectInternal.Header.TotalDifficulty = difficulty;
            return this;
        }
        
        public BlockBuilder WithDifficulty(BigInteger difficulty)
        {
            TestObjectInternal.Header.Difficulty = difficulty;
            return this;
        }
        
        public BlockBuilder WithParent(Block block)
        {
            TestObjectInternal.Header.Number = (block?.Number ?? -1) + 1;
            TestObjectInternal.Header.ParentHash = block == null ? Keccak.Zero : block.Hash;
            return this;
        }
        
        public BlockBuilder WithParentHash(Keccak parent)
        {
            TestObjectInternal.Header.ParentHash = parent;
            return this;
        }
        
        public BlockBuilder Genesis => WithNumber(0);

        protected override void BeforeReturn()
        {
            base.BeforeReturn();
            TestObjectInternal.Header.Hash = BlockHeader.CalculateHash(TestObjectInternal.Header);
        }
    }
}