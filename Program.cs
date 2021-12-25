using Microsoft.Diagnostics.Runtime;
using System;
using System.Linq;

namespace memleak
{
    class Program
    {
        static void Main(string[] args)
        {
            int pid = int.Parse(args[0]);

            while (true)
            {
                EnumRegions(pid);

                GC.Collect();

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }
        private static void EnumRegions(int pid)
        {
            using var target = DataTarget.CreateSnapshotAndAttach(pid);

            foreach (var clrVersion in target.ClrVersions)
            {
                var runtime = clrVersion.CreateRuntime();

                TouchOtherRegions(runtime);
            }
        }

        private static void TouchOtherRegions(ClrRuntime runtime)
        {
            var heap = runtime.Heap;
            heap.EnumerateRoots().Count();
        }
    }
}
