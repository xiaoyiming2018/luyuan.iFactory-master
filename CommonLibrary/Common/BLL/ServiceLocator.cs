using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public static class ServiceLocator
    {
        public static IServiceProvider Instance { get; set; }
    }
}
