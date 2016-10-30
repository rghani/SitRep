using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.BodyBasics
{
    class PhotonPackage
    {
        private BitVector32 postureFlagBits { get; set; }
        public DateTime triggeredTime;
        public string eventName { get; set; }
        public PhotonPackage(BitVector32 passedFlagBits)
        {
            //When photon package is created, record the time at which it was created
            postureFlagBits = passedFlagBits;
            triggeredTime = DateTime.Now;
        }
    }
}
