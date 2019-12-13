using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // Editor linkage.
    public Transform wordsParentTransform;
    public GameObject wordObjPrefab;
    public Text scoreIndicator;
    public StatTracker st;

    // Public variables.
    public static GameController instance;
    public WordComponent LockedInWord { get; set; }

    // Private variables.
    int multiplier = 1;
    int letterStreak = 0;
    int fails=0;
    float spawnFrequency = 10f; // TODO not hardcoded, but by level?
    int maxFails = 3; // TODO not hardcoded, but per level or something?
    float timeSinceLastSpawn = float.PositiveInfinity;
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "Not read only!")]
    List<WordComponent> ActiveWords = new List<WordComponent>();


    // DEBUG variables.
    public Text debugprint;
    readonly string[] wordList =
    {
        "Poo",
        "Test",
        "Love",
        "Christmas",
        "Cat",
        "Dog",
        "Zoom",
        "Map",
        "Far",
        "Short",
        "RIP"
    };

    void Start()
    {
		// Make sure there's only one of us.
		if (instance == null) {
			instance = this;
		}else if (instance != this){
            // We are not The One(tm).
            Destroy(this);
        }

        // Clean visuals.
        scoreIndicator.text = "Score: 0\nStreak: 0\nMult: 1x";
    }

    void Update()
    {
        // DEBUG print
        timeSinceLastSpawn += Time.deltaTime;
        debugprint.text = "DEBUG:\n"+timeSinceLastSpawn.ToString()+"\n"+Time.timeSinceLevelLoad.ToString()+"\n"+st.TotalFails;
        if (timeSinceLastSpawn > spawnFrequency)
        {
            CreateWord(wordList[Random.Range(0, wordList.Length)]);
            timeSinceLastSpawn = 0;
        }
    }

    // This Unity function grabs all keyboard events, among other things.
    void OnGUI()
    {
        // Let's make sure this is a keyboard event, specifically a NEW keystroke.
        if (Event.current.isKey && Event.current.type == EventType.KeyDown)
        {
            if (WordComponent.keycodeDict.ContainsValue(Event.current.keyCode))
            {
                //This is a letter. Do we have a locked-in word?
                if (LockedInWord != null)
                {
                    //Is it the correct letter?
                    if (Event.current.keyCode == LockedInWord.GetNextLetter())
                    {
                        st.ChangeLettersTyped(1);
                        LockedInWord.NextLetterTyped();
                    }
                    else
                    {
                        // WRONG!
                        CountFail();
                    }
                }
                else
                {
                    // No active word, let's find one!
                    foreach (WordComponent word in ActiveWords)
                    {
                        if(word.enabled == false)
                        {
                            // Don't grab onto words that are disabled.
                            continue;
                        }

                        if (word.GetNextLetter() == Event.current.keyCode)
                        {
                            // This word starts with the typed letter, it's now active.
                            st.ChangeLettersTyped(1);
                            word.NextLetterTyped();
                            break;
                        }
                    }
                    if (LockedInWord == null)
                    {
                        CountFail();
                    }
                }
            }
        }
    }

    void CreateWord(string word)
    {
        // Create Game Object.
        GameObject go = GameObject.Instantiate(wordObjPrefab, wordsParentTransform);
        // Name Game Object.
        go.name = word;

        // Initiate the word controller script.
        go.GetComponent<WordComponent>().OurWord = word;

        // Register the word with us.
        ActiveWords.Add(go.GetComponent<WordComponent>());

        // Track this new word.
        st.ChangeLettersSpawned(word.Length);
        st.ChangeWordsSpawned(1);

        // Go juice, go!
        go.SetActive(true);
    }

    public void RemoveWordFromPlay(WordComponent go, bool completed)
    {
        if (ActiveWords.Contains(go) == false)
        {
            Debug.LogError("Trying to remove a word from play, that isn't in play!");
            return;
        }
        if (completed)
        {
            st.ChangeWordsTyped(1);
        }
        ActiveWords.Remove(go);
    }

    public void ScorePoints(int points, bool addStreak = true)
    {
        // Add score BEFORE changing multiplier.
        st.ChangeScore(points * multiplier);

        /* Check if we need to up the multiplier level, as per chart below:
         Multiplier levels: 1x, 2x, 3x, 5x, 10x.
         Streak length: 5 letters, 10 letters, 25 letters, 50 letters.
         */
        if (addStreak)
        {
            letterStreak++;

            //Update our record.
            if (letterStreak > st.HighestStreak) {
                // This uses a delta value, so we need to remove the original number to create a new absolute.
                // Or just use a set method, TBD.
                st.ChangeHighestStreak(letterStreak - st.HighestStreak);
            }

            // And our multiplier.
            if (letterStreak == 50)
            {
                multiplier = 10;
            }
            else if (letterStreak == 25)
            {
                multiplier = 5;
            }
            else if (letterStreak == 10)
            {
                multiplier = 3;
            }
            else if (letterStreak == 5)
            {
                multiplier = 2;
            }
        }
        UpdateScoreBoard();
    }

    void CountFail()
    {
        fails++;
        st.ChangeTotalFails(1);

        // Are we game over?
        if(fails >= maxFails)
        {
            SceneManager.LoadScene("Game Over", LoadSceneMode.Single);
        }

        //TODO Some fancy graphics thing.

        // Reset the letter streak.
        letterStreak = 0;
        // AND the combo multiplier.
        multiplier = 1;
        UpdateScoreBoard();
    }

    void UpdateScoreBoard()
    {
        // Update the display.
        scoreIndicator.text = string.Format("Score: {0:N0}\nStreak: {1:N0}\nMult: {2:N0}x",st.Score, letterStreak, multiplier);
    }
}
