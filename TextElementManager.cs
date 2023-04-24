using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace ProgramStartup
{
    internal class TextElementManager
    {
        private const string ConfigFileName = "SaveFile.xml";
        private const int MinElements = 1;

        private List<RichTextBox> textElements;
        private int elementIndex = 1;

        public int ElementIndex => elementIndex;

        public TextElementManager(List<RichTextBox> inputBoxes)
        {
            textElements = inputBoxes;

            for (int i = elementIndex; i < textElements.Count; i++)
            {
                textElements[i].Enabled = false;
            }
        }

        public void AddElement()
        {
            if (elementIndex >= textElements.Count) return;

            textElements[elementIndex].Enabled = true;
            elementIndex++;
        }

        public void RemoveElement()
        {
            if (elementIndex <= MinElements) return;

            elementIndex--;
            textElements[elementIndex].Enabled = false;
        }

        public void RunCommands()
        {
            for (int i = 0; i < elementIndex; i++)
            {
                var process = new Process();
                process.StartInfo.UseShellExecute = false;

                if (Environment.OSVersion.Version.Major >= 6)
                {
                    process.StartInfo.Verb = "runas";
                }

                process.StartInfo.FileName = textElements[i].Text;
                process.Start();
            }
        }

        public void ReadState()
        {
            if (File.Exists(ConfigFileName))
            {
                LoadSaveState();
            }
        }

        public void WriteState()
        {
            WriteSaveState();
        }

        public void SetTextOfCurrentElement(string directoryText)
        {
            textElements[ElementIndex - 1].Text = directoryText;
        }

        private void LoadSaveState()
        {
            var serializer = new XmlSerializer(typeof(EndStateSaver));
            using (FileStream fileStream = File.OpenRead(ConfigFileName))
            {
                var state = (EndStateSaver)serializer.Deserialize(fileStream);
                var index = state.ElementIndex;

                for (int i = 0; i < index; i++)
                {
                    textElements[i].Text = state.TextElements[i];
                    textElements[i].Enabled = true;
                }

                elementIndex = index;
            }
        }

        private void WriteSaveState()
        {
            using (StreamWriter writer = new StreamWriter(ConfigFileName))
            {
                var state = new EndStateSaver();
                state.ElementIndex = elementIndex;

                var list = new List<string>();
                for(int i = 0; i < elementIndex; i++)
                {
                    list.Add(textElements[i].Text);
                }

                state.TextElements = list;

                var serializer = new XmlSerializer (typeof(EndStateSaver));
                serializer.Serialize(writer, state);
            }
        }
    }
}
