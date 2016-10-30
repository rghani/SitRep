using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.BodyBasics
{
    class EventPipeline<PhotonPackage>
    {
        public Queue<PhotonPackage> queue = new Queue<PhotonPackage>(30);
        public event EventHandler OnEnqueue;
        public event EventHandler OnDequeue;


        
        protected virtual void InitiateEnqueueEvent()
        {
            if (OnEnqueue != null) OnEnqueue(this, EventArgs.Empty);
        }
        public virtual PhotonPackage Enqueue(PhotonPackage item)
        {
            PhotonPackage prevItem = queue.Peek();
            //Comapre previous item and current item to be queued
            queue.Enqueue(item);
            InitiateEnqueueEvent();
            return item;
        }

        protected virtual void InitiateDequeueEvent()
        {
            if (OnDequeue != null) OnDequeue(this, EventArgs.Empty);
        }

        public virtual PhotonPackage Dequeue()
        {
            //Check some object property which is set in the onEnqueue event, once its confirmed that the event is published, then empty the queue
            PhotonPackage item = queue.Dequeue();
            InitiateDequeueEvent();
            return item;
        }
    }
}
