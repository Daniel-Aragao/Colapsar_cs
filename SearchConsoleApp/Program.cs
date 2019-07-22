using System;
using Core.models;

namespace SearchConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(EPathStatus.SourceAndTargetAreToCloseToCollapse.ToString());
            Console.WriteLine("Number Of Logical Processors: {0}", Environment.ProcessorCount);
        }
    }
}
//   <ItemGroup>
//     <ProjectReference Include="..\Core\Core.csproj" />
//     <ProjectReference Include="..\Infra\Infra.csproj" />
//   </ItemGroup>
