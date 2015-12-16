using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.Core
{
    [Serializable]
    public class ControlPath
    {

        public ControlPath()
        {
            Path = new List<ControlDescription>();
        }

        [JsonConstructor]
        public ControlPath(List<ControlDescription> Path)
        {
            if (Path != null) this.Path = new List<ControlDescription>(Path);
        }

        public ControlPath(params ControlDescription[] Path)
        {
            if (Path != null) this.Path = new List<ControlDescription>(Path);
        }

        public List<ControlDescription> Path { get; protected set; }

        //[JsonIgnore]
        //public Boolean HasRelative { get { return GetRelativePath.Path.Count > 0; } }

        //[JsonIgnore]
        //public Descriptor Window { get { return Path.FirstOrDefault(); } }

        public ControlPath GetRelativePath()
        {

            ControlPath relative = new ControlPath();

            var descriptors = new List<ControlDescription>(this.Path);

            if (descriptors.Count > 0)
            {

                descriptors.RemoveAt(0);

                relative.Path.AddRange(descriptors);

            }

            return relative;

        }

        public override string ToString()
        {
            StringBuilder locationBuilder = new StringBuilder();

            List<String> controlDescription = new List<string>();

            foreach (var descriptor in Path)
            {

                controlDescription.Add(descriptor.ToString());

            }

            locationBuilder.Append(String.Join("\\", controlDescription.ToArray()));

            return locationBuilder.ToString();
        }
    }
}
