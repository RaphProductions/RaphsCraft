using Mono.Cecil;
using System.Diagnostics;

namespace Mixin.NET
{
    public class MixinLoader
    {
        #region Fields
        private string _directory;
        private string _inputAssembly;
        private string _outputAssembly;
        private AssemblyDefinition _inputAssemblyDef;
        private List<AssemblyDefinition> _mixinDllsDef;
        #endregion

        #region Properties
        /// <summary>
        /// Directory that contains the DLL, who themselves contains the mixins. 
        /// </summary>
        public string Directory => _directory;

        /// <summary>
        /// The DLL to injects the mixins of _directory in.
        /// </summary>
        public string InputAssembly => _inputAssembly;

        /// <summary>
        /// The output for the injected DLL.
        /// </summary>
        public string OutputAssembly => _inputAssembly;
        #endregion

        #region Constructor
        public MixinLoader(string directory, string inputAssembly, string? outputAssembly = null)
        {
            if (outputAssembly == null)
                outputAssembly = Path.GetTempFileName();

            _directory = directory;
            _outputAssembly = outputAssembly;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Load all the mixins in the DLLs of _directory, injects the mixins into the classes of _inputAssembly,
        /// then write the final assembly to _outputAssembly.
        /// </summary>
        public void Load()
        {
            /// STEP 1: load DLLs
            Console.WriteLine("(MixinLoader/INFO) Loading all DLLs...");

            // Load the DLL to inject
            _inputAssemblyDef = AssemblyDefinition.ReadAssembly(_inputAssembly);

            // Load the DLLs in the directory
            _mixinDllsDef = new();
            foreach (string dll in System.IO.Directory.GetFiles(_directory))
                _mixinDllsDef.Add(AssemblyDefinition.ReadAssembly(dll));


            /// Finally, write the output assembly def.
            _inputAssemblyDef.Write(_outputAssembly);
        }
        #endregion
    }
}
