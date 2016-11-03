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
        public BitVector32 postureFlagBits;
        public DateTime triggeredTime;
        public string eventName;
        public PhotonPackage(BitVector32 passedFlagBits)
        {
            //When photon package is created, record the time at which it was created
            postureFlagBits = passedFlagBits;
            triggeredTime = DateTime.Now;
        }
    }
}
