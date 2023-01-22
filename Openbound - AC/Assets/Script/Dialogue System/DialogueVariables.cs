using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class DialogueVariables
{

    public Dictionary<string, Ink.Runtime.Object> variables { get; private set; }

    Story globalVaraiblesStory;

    const string saveVariables = "INK_VARIABLES";

    public DialogueVariables(TextAsset loadGlobalsJSON) {
        globalVaraiblesStory = new Story(loadGlobalsJSON.text);

        if(PlayerPrefs.HasKey(saveVariables))
        {
            string jsonState = PlayerPrefs.GetString(saveVariables);
            globalVaraiblesStory.state.LoadJson(jsonState);
        }

        variables = new Dictionary<string, Ink.Runtime.Object>();
        foreach (string  name in globalVaraiblesStory.variablesState)
        {
            Ink.Runtime.Object value = globalVaraiblesStory.variablesState.GetVariableWithName(name);
            variables.Add(name, value);
            Debug.Log("Initialized global dialogue variable: " + name + " = " + value);
        }
    }

    public void SaveVariables()
    {
        if(globalVaraiblesStory != null)
        {
            VariablesToStory(globalVaraiblesStory);
            PlayerPrefs.SetString(saveVariables, globalVaraiblesStory.state.ToJson());
        }
    }

    public void StartListening(Story story)
    {
        VariablesToStory(story);
        story.variablesState.variableChangedEvent += VariableChanged;
    }

    public void StopListening(Story story)
    {
        story.variablesState.variableChangedEvent -= VariableChanged;
    }

    void VariableChanged(string name, Ink.Runtime.Object value)
    {
        if (variables.ContainsKey(name))
        {
            variables.Remove(name);
            variables.Add(name, value);
        }
    }

    void VariablesToStory(Story story)
    {
        foreach (KeyValuePair<string, Ink.Runtime.Object> variable in variables)
        {
            story.variablesState.SetGlobal(variable.Key, variable.Value);
        }
    }

}
