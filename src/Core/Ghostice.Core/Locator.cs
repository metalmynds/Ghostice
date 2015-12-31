using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghostice.Core
{
    [Serializable]
    public class Locator
    {

        public Locator()
        {
            this.Path = new List<Descriptor>();
        }

        [JsonConstructor]
        public Locator(List<Descriptor> path)
        {
            if (path != null) this.Path = new List<Descriptor>(path);
        }

        public Locator(params Descriptor[] path)
        {
            if (path != null) this.Path = new List<Descriptor>(path);
        }

        public List<Descriptor> Path { get; protected set; }

        //[JsonIgnore]
        //public Boolean HasRelative { get { return GetRelativePath.Path.Count > 0; } }

        //[JsonIgnore]
        //public Descriptor Window { get { return Path.FirstOrDefault(); } }

        public Locator GetRelativePath()
        {

            Locator relative = new Locator();

            var descriptors = new List<Descriptor>(this.Path);

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

            foreach (var descriptor in this.Path)
            {

                controlDescription.Add(descriptor.ToString());

            }

            locationBuilder.Append(String.Join("\\", controlDescription.ToArray()));

            return locationBuilder.ToString();
        }
    }
}
