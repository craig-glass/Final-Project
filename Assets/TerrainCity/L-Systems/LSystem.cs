using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DefaultNamespace;
using System.Text;
using System.Diagnostics;

/*
 * axiom = F
 * F -> FF+[+F-F-F]-[-F+F+F]
 * angle = 22.5
 */


public class LSystem
{
    Stopwatch Timer = new Stopwatch();

    string sentence;
    Dictionary<string, string> ruleset;
    Dictionary<string, Action<Turtle>> turtleCommands;
    Turtle turtle;

    public LSystem(StringBuilder axiom, Dictionary<string, string> ruleset, Dictionary<string, Action<Turtle>> turtleCommands, Vector3 initialPos, Quaternion orientation)
    {
        sentence = axiom.ToString();
        this.ruleset = ruleset;
        this.turtleCommands = turtleCommands;

        turtle = new Turtle(initialPos, orientation);
    }

    public void DrawSystem()
    {
        Timer.Start();

        foreach (var instruction in sentence)
        {
            if (turtleCommands.TryGetValue(instruction.ToString(), out var command))
            {
                command(turtle);
                //UnityEngine.Debug.Log("position = " + turtle.Position);
                //yield return null;
               // yield return new WaitForSeconds(0.5f);
            }
        }
        Timer.Stop();
        UnityEngine.Debug.Log("DrawSystem time taken: " + Timer.Elapsed);
    }

    public string GenerateSentence()
    {
        sentence = IterateSentence(sentence);
        return sentence;
    }

    string IterateSentence(string oldSentence)
    {
        var newSentence = "";

        //generate/iterate

        foreach (var c in oldSentence)
        {
            if (ruleset.TryGetValue(c.ToString(), out string replacement))
            {
                newSentence += replacement;
            }
            else
            {
                newSentence += c;
            }
        }

        return newSentence;

    }

    
}
