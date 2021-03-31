using System.Collections.Generic;

namespace Utilities.ApnsNotifications
{
    public class ThreadSafeQueue<T>
    {
        readonly Queue<T> queue;
        readonly object lockObj;

        public ThreadSafeQueue()
        {
            queue = new Queue<T>();
            lockObj = new object();
        }

        public T Dequeue()
        {
            lock (lockObj)
            {
                return queue.Dequeue();
            }
        }

        public void Enqueue(T item)
        {
            lock (lockObj)
            {
                queue.Enqueue(item);
            }
        }

        public int Count
        {
            get { return queue.Count; }
        }
    }
}