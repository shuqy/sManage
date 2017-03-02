using Core.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core {
    public class AppContext {
        private static IApplication _application;
        public static void Start(IApplication application) {
            _application = application;
        }
        public static IApplication Current {
            get { return _application; }
        }
    }
}
