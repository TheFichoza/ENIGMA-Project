    public class Key
    {
        public int[] rotor_config = { 1, 2, 3 }, start_position = { 1, 1, 1 };
        public double ioc;
        public decimal hillClimb;
        public string text;
        public List<string> plugboard = new List<string>();
        public Key(int[] rotor_config, int[] positions, int[] histogram, string text)
        {
            this.rotor_config = rotor_config;
            start_position[0] = positions[0];
            start_position[1] = positions[1];
            start_position[2] = positions[2];
            ioc = IoC(histogram);
            this.text = text;
        }
        public Key(int[] rotor_config, int[] positions, int[] histogram)
        {
            this.rotor_config = rotor_config;
            start_position[0] = positions[0];
            start_position[1] = positions[1];
            start_position[2] = positions[2];
            ioc = IoC(histogram);
            text = "";
        }
        public Key()
        {
            ioc = -1;
            text = "";
        }
        public double IoC(int[] histogram)
        {
            double output = 0;
            for (int i = 0; i < 26; i++)
            {
                if (histogram[i] != 0) output += (histogram[i] * (histogram[i] - 1));
            }
            output /= (histogram.Sum() * (histogram.Sum() - 1));
            output = Math.Round(output, 7);
            return output;
        }
        public override string ToString()
        {
            string msg=string.Join(", ", plugboard);
            if(plugboard.Count==0) msg = "NONE";
            return $"(IOC: {ioc}\nCHOSEN ROTORS: {string.Join(", ", rotor_config)}\nPOSITIONS: {string.Join(", ", start_position.Select(x => x + 1))}\nPLUGS: {msg}\n\n{text}\n\n";
        }
    }