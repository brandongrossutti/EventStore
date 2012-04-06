using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GHI.EventStore.Tests
{
    public class EventStoreSessionFinder
    {
        private static EventStoreSessionFinder _context;
        public EventStoreSessionFinder()
        {
        }

        private void SetProperties()
        {
            //IRepository eventStore 
        }

        public static EventStoreSessionFinder Current
        {
            get
            {
                if (_context == null)
                {
                    _context = new EventStoreSessionFinder();
                    _context.SetProperties();

                }
                return _context;
            }
        }
    }
}
