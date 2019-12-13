using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordComponent : MonoBehaviour
{

    // Statics
    public readonly static Dictionary<char,KeyCode> keycodeDict =
        new Dictionary<char, KeyCode> {
            {'a', KeyCode.A },
            {'b', KeyCode.B },
            {'c', KeyCode.C },
            {'d', KeyCode.D },
            {'e', KeyCode.E },
            {'f', KeyCode.F },
            {'g', KeyCode.G },
            {'h', KeyCode.H },
            {'i', KeyCode.I },
            {'j', KeyCode.J },
            {'k', KeyCode.K },
            {'l', KeyCode.L },
            {'m', KeyCode.M },
            {'n', KeyCode.N },
            {'o', KeyCode.O },
            {'p', KeyCode.P },
            {'q', KeyCode.Q },
            {'r', KeyCode.R },
            {'s', KeyCode.S },
            {'t', KeyCode.T },
            {'v', KeyCode.V },
            {'u', KeyCode.U },
            {'w', KeyCode.W },
            {'x', KeyCode.X },
            {'y', KeyCode.Y },
            {'z', KeyCode.Z }};

    // Editor linkage
    public GameObject letterObjPrefab;


    // Public members.
    public string OurWord;

    // Private members.
    int letterIndex;
    Text[] letterObjects;

    private void OnEnable()
    {
        if(OurWord == null || OurWord == "")
        {
            Debug.LogError("Woke up a word that doesn't have a word assigned yet.");
            this.enabled = false;
            return;
        }

        if (letterObjects == null)
        {
            letterObjects = new Text[OurWord.Length];

            Transform cnvs = this.GetComponentInChildren<Canvas>().transform;

            float wordStartX = OurWord.Length * -8.5f;

            for (int i=0; i < OurWord.Length; i++)
            {
                // Create the letter.
                GameObject ltrGO = GameObject.Instantiate(letterObjPrefab, cnvs);

                // Rename the game object.
                ltrGO.name = i + " " + OurWord.Substring(i, 1);

                // Position the letter.
                ltrGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(17 * (i + 0.5f) + wordStartX, 0);

                // Grab the text component and store it.
                letterObjects[i] = ltrGO.GetComponent<Text>();

                // Display the correct letter.
                letterObjects[i].text = OurWord.Substring(i, 1);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Are we supposed to disappear?
        if (letterIndex == OurWord.Length)
        {
            // Release input lock.
            GameController.instance.LockedInWord = null;
            // Inform the GameController we're donezo.
            GameController.instance.RemoveWordFromPlay(this, true);

            // Destroy ourselves.
            this.enabled = false;
            return;
        }
    }
    public void NextLetterTyped() {
        // Take input lock if not ours already.
        if (GameController.instance.LockedInWord != this)
        {
            GameController.instance.LockedInWord = this;
        }

        // Progress and scoring.
        letterIndex++;
        GameController.instance.ScorePoints(1);

        // Graphical things.
        letterObjects[letterIndex-1].color = Color.red;


        // Did we complete the word?
        if(letterIndex == OurWord.Length)
        {
            // Score points.
            GameController.instance.ScorePoints(OurWord.Length, false);

            // TODO Shiny graphics maybe?
        }
    }
    

    public KeyCode GetNextLetter() {
        // Grab the index.
        string substr = OurWord.Substring(letterIndex, 1);

        // Ensure it's lowercase.
        substr = substr.ToLower();

        // Turn it into a character.
        char character = substr.ToCharArray()[0];

        // Look it up and return.
        return keycodeDict[character];
    }
}
