﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.BodyBasics
{
    class EventPipeline
    {
        public Queue<PhotonPackage> queue = new Queue<PhotonPackage>(30);
        public event EventHandler OnEnqueue;
        public event EventHandler OnDequeue;


        
        protected void InitiateEnqueueEvent()
        {
            if (OnEnqueue != null) OnEnqueue(this, EventArgs.Empty);
        }
        public virtual PhotonPackage EnqueuePipeline(PhotonPackage item)
        {
            if(queue.Count > 0)
            {
                PhotonPackage prevItem = queue.Peek();
                //Compare previous item and current item to be queued, if they are different then enqueue the new photon package to be sent
                if (prevItem.postureFlagBits.Data != item.postureFlagBits.Data)
                {
                    queue.Enqueue(item);
                    InitiateEnqueueEvent();
                    return item;
                }
                return null;
            }
            else
            {
                queue.Enqueue(item);
                return item;
            }
        }

        protected void InitiateDequeueEvent()
        {
            if (OnDequeue != null) OnDequeue(this, EventArgs.Empty);
        }

        public virtual PhotonPackage DequeuePipeline()
        {
            PhotonPackage item = queue.Dequeue();
            InitiateDequeueEvent();
            return item;
        }
    }
}
