using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7D2D_ServerInfo
{
    interface IGUI:IDisposable
    {
        void Draw();
        void DrawConnectionError();
        bool UpdateAvailable { get; set; }
    }
}
