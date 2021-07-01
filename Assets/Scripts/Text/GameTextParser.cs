using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTextParser : MonoBehaviour
{
    public TextAsset gameText;

    // Start is called before the first frame update
    void Awake()
    {
        string[] lines = gameText.text.Split('\n');
        for (int i = 0; i < lines.Length; ++i)
        {
            string line = lines[i].Trim();
            if (line[0] == '[' && line[line.Length - 1] == ']')
            {
                ParseCommand(line);
            }
        }
    }

    private void ParseCommand(string line)
    {

    }
}