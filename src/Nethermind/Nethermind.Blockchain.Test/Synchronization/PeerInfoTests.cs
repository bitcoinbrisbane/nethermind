//  Copyright (c) 2018 Demerzel Solutions Limited
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

namespace Nethermind.Blockchain.Test.Synchronization
{
    public class PeerInfoTests
    {
        public void Should_Report_Peers()
        {
            // * [Peer |178.128.117.242:30303| 9136207|  47 kbps|Parity]
            // * [Peer | 35.243.195.135:33302| 9136207| 133 kbps|Parity]
            // * [Peer |  52.196.62.203:30303| 9136204|  46 kbps|Geth]
            // * [Peer |   54.85.105.50:30303|       0|  10 kbps|Geth]
        }
    }
}