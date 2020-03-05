using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Load : MonoBehaviour
{
    // Variables
    private Stream inStream;

    private PlayerInventory pInventoryScript;
    [SerializeField] private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        pInventoryScript = player.GetComponent<PlayerInventory>();

        LoadRecipeList(); // load recipes text file

        Debug.Log(pInventoryScript.recipeList[0]);
        Debug.Log(pInventoryScript.recipeList[0].outputString);
        Debug.Log(pInventoryScript.recipeList[1].type);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadRecipeList()
    {
        inStream = File.OpenRead("Assets/Resources/Recipes.txt");

        // txt file reading vars
        string thisRow;
        int lineNumber = 0;
        int numberPerEntry = 5;

        // recipe vars
        int recipeIndex = 0; // number of recipe
        Recipe.recipeType type = Recipe.recipeType.mod; // tool or mod?
        string output = ""; // output from thingy
        Dictionary<string, int> inputs = new Dictionary<string, int>(); // recipe inputs
        int extraResource = 0; // for optionals (gold nozzle, iron loader)

        try
        {
            StreamReader sr = new StreamReader(inStream);
            while ((thisRow = sr.ReadLine()) != null)
            {
                switch (lineNumber)
                {
                    case 0: // line 1
                        int.TryParse(thisRow, out recipeIndex);

                        lineNumber++;
                        break;
                    case 1: // line 2
                        if (thisRow == "mod")
                        {
                            type = Recipe.recipeType.mod;
                        }
                        else
                        {
                            type = Recipe.recipeType.tool;
                        }

                        lineNumber++;
                        break;
                    case 2: // line 3
                        string[] ingredients = thisRow.Split(',', '|'); //split the line into an array

                        inputs = new Dictionary<string, int>(); // init inputs dic

                        for (int i = 0; i < ingredients.Length; i+=2) // skip by two -> "string, int, string, int,..."
                        {
                            int numIngredients;
                            int.TryParse(ingredients[i + 1], out numIngredients); // parse number
                            inputs.Add(ingredients[i], numIngredients);
                        }

                        lineNumber++;
                        break;
                    case 3:
                        int.TryParse(thisRow, out extraResource);

                        lineNumber++;
                        break;
                    case 4:
                        output = thisRow;

                        lineNumber++;
                        break;
                }
                if (lineNumber == numberPerEntry)
                {
                    Recipe newRecipe = new Recipe(inputs, type, recipeIndex, output);
                    pInventoryScript.recipeList.Add(newRecipe);

                    thisRow = sr.ReadLine(); // skip the gap line

                    lineNumber = 0;
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error reading recipes: " + e.Message);
        }
        finally
        {
            inStream.Close();
        }
    }
}
