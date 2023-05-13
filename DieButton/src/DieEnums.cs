using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DieButton
{
    internal class DieEnums
    {
        public static void RegisterValues()
        {
            DWTD = new SoundID("DWTD", true);
            rot_tor = new SoundID("rot_tor", true);
            sleepy = new SoundID("sleepy", true);
            IDWTD = new SoundID("IDWTD", true);

        }
        public static void UnregisterValues()
        {
            Unregister(DWTD);
            Unregister(DWTD);
            Unregister(DWTD);
            Unregister(DWTD);

        }

        private static void Unregister<T>(ExtEnum<T> extEnum) where T : ExtEnum<T>
        {
            extEnum?.Unregister();
        }

        public static SoundID DWTD;
        public static SoundID rot_tor;
        public static SoundID sleepy;
        public static SoundID IDWTD;
    }
}
