using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace DefaultNamespace
{
    public class DrawEstate : MonoBehaviour
    {

        /*
     * axiom = F
     * F -> FF+[+F-F-F]-[-F+F+F]
     * angle = 22.5
     */


        Stopwatch Timer = new Stopwatch();
        int iterations;
        Dictionary<string, string> chosenRuleset;
        Dictionary<string, Action<Turtle>> chosenCommands;

        Dictionary<string, string> ruleset = new Dictionary<string, string>
        {
            {"A", "F" },
            {"B", "AAAAA" },
            {"C", "f" },
            {"D", "[GH]AD" },
            {"E", "[HG]AE" },
            {"G", "AA" },
            {"H", "AA" },
            {"I", "C[--G]I" },
            {"J", "C[++G]J" }
        };

        Dictionary<string, Action<Turtle>> commands = new Dictionary<string, Action<Turtle>>
        {
            {"F", turtle => turtle.Translate(new Vector3(0, 0, 20))},
            {"f", turtle => turtle.Translate(new Vector3(0, 0, 60)) },
            {"O", turtle => turtle.TranslateOffset(new Vector3(0, 0, 10)) },
            {"o", turtle => turtle.TranslateOffset(new Vector3(0, 0, -10)) },
            {"P", turtle => turtle.TranslateOffset(new Vector3(0, 0, 30)) },
            {"p", turtle => turtle.TranslateOffset(new Vector3(0, 0, -30)) },
            {"+", turtle => turtle.Rotate(new Vector3(0, 45f, 0)) },
            {"-", turtle => turtle.Rotate(new Vector3(0, -45f, 0)) },
            {"[", turtle => turtle.Push() },
            {"]", turtle => turtle.Pop() }
        };

        Dictionary<string, string> rulesetCircle = new Dictionary<string, string>
        {
            {"Z", "A[C]B[D]AA[C]BB[D]" },
            {"A", "F+F+F+" },
            {"B", "f-f-f-" },
            {"C", "-" },
            {"D", "+"}
        };

        Dictionary<string, Action<Turtle>> commandsCircle = new Dictionary<string, Action<Turtle>>
        {
            {"F", turtle => turtle.Translate(new Vector3(0.35f, 0, 1.5f))},
            {"f", turtle => turtle.Translate(new Vector3(-0.35f, 0, 1.5f))},
            {"+", turtle => turtle.Rotate(new Vector3(0, 25f, 0)) },
            {"-", turtle => turtle.Rotate(new Vector3(0, -25f, 0)) },
            {"[", turtle => turtle.Push() },
            {"]", turtle => turtle.Pop() }
        };

        Dictionary<string, string> roundaboutRuleset = new Dictionary<string, string>
        {
            {"A", "[EDED]++[DEDE]++DD++A" },
            {"B", "FFFFF" },
            {"C", "111111111" },
            {"D", "2p[P2p[++OC[--OC]C[--OC]Co[--OC]]]" },
            {"E", "Co[--OC[++OC]Co[++OC]]OCo[++OC]" },
        };

        Dictionary<string, string> testRuleset = new Dictionary<string, string>
        {
            {"A", "1[o++O11o--O1]11[o--O11o++O1]11A" }
        };

        Dictionary<string, Action<Turtle>> roundaboutCommands = new Dictionary<string, Action<Turtle>>
        {
            {"R", turtle => turtle.TranslateRoundabout(new Vector3(0, 0, 24.3f)) },
            {"S", turtle => turtle.ExitRoundabout(new Vector3(44.3f, 0, -24.3f)) },
            {"T", turtle => turtle.ExitRoundabout(new Vector3(44.3f, 0, -44.3f)) },
            {"+", turtle => turtle.Rotate(new Vector3(0, 45f, 0)) },
            {"-", turtle => turtle.Rotate(new Vector3(0, -45f, 0)) },
            {"F", turtle => turtle.Translate(new Vector3(0, 0, 2)) },
            {"1", turtle => turtle.Translate(new Vector3(0, 0, 20)) },
            {"2", turtle => turtle.Translate(new Vector3(0, 0, 60)) },
            {"O", turtle => turtle.TranslateOffset(new Vector3(0, 0, 14)) },
            {"o", turtle => turtle.TranslateOffset(new Vector3(0, 0, -14)) },
            {"P", turtle => turtle.TranslateOffset(new Vector3(0, 0, 30)) },
            {"p", turtle => turtle.TranslateOffset(new Vector3(0, 0, -30)) },
            {"[", turtle => turtle.Push() },
            {"]", turtle => turtle.Pop() },
        };

        // Start is called before the first frame update
        void Start()
        {
            Vector3 pos;
            Timer.Start();

            foreach (KeyValuePair<Vector2, Quaternion> edge in VoronoiRoads.edges)
            {
                //iterations = UnityEngine.Random.Range(3, 6);
                iterations = 8;
                
                StringBuilder sb = new StringBuilder();
                if (UnityEngine.Random.Range(1, 2) == 0)
                {
                    sb = Axiom(sb);
                    chosenRuleset = ruleset;
                    chosenCommands = commands;
                }
                else
                {
                    sb = RoundaboutAxiom(sb);
                    chosenRuleset = roundaboutRuleset;
                    chosenCommands = roundaboutCommands;
                }
                bool tooclose = false;

                foreach (KeyValuePair<Vector2, Quaternion> otherEdge in VoronoiRoads.edges)
                {
                    if (edge.Key == otherEdge.Key) continue;

                    if (Vector3.Distance(edge.Key, otherEdge.Key) < 400)
                    {
                        tooclose = true;
                        break;
                    }
                    //if (Vector3.Distance(edge.Key, otherEdge.Key) < 600)
                    //{
                    //    iterations = UnityEngine.Random.Range(3, 8);
                    //}
                }

                if (tooclose)
                {
                    continue;
                }
                pos = new Vector3(edge.Key.x, 50, edge.Key.y);

                var lSystem = new LSystem(sb, chosenRuleset, chosenCommands, pos, edge.Value);

                while (iterations > 0)
                {
                    RoundaboutAxiom(sb);
                    // UnityEngine.Debug.Log(lSystem.GenerateSentence());
                    lSystem.GenerateSentence();
                    iterations--;
                }

                lSystem.DrawSystem();                
            }
            Timer.Stop();
            UnityEngine.Debug.Log("Time taken to draw all L-Systems: " + Timer.Elapsed);
        }

        // string builder for grid-like roads
        StringBuilder Axiom(StringBuilder sb)
        {
            sb = new StringBuilder();
            int roadBlocks = iterations - 1;
            int lengthOfBlocks =  8 - roadBlocks;
            StringBuilder rulesetI = new StringBuilder();
            StringBuilder rulesetG = new StringBuilder();
            int index = UnityEngine.Random.Range(1, 6);
            int gAmount = 0;
            bool firstLoop = true;

            roadBlocks *= lengthOfBlocks;

            gAmount = UnityEngine.Random.Range(1, 4);
            while (gAmount > 0)
            {
                rulesetG.Append("AA");
                gAmount--;
            }

            while (lengthOfBlocks > 0)
            {
                rulesetI.Append("C");
                lengthOfBlocks--;
            }

            ruleset["I"] = rulesetI + "[--G]I";
            ruleset["J"] = rulesetI + "[++G]J";

            if (UnityEngine.Random.Range(0, 100) < 50)
            {

                sb.Append("[");
            Loop:
                if (firstLoop)
                {
                    sb.Append("[");
                    firstLoop = false;
                }
                else
                {
                    sb.Append("o[");
                }

                ruleset["I"] = rulesetI + "p[--O" + rulesetG + "]PI";

                sb.Append("++PI");

                sb.Append("]O" + rulesetG);

                while (index > 0)
                {

                    index--;
                    goto Loop;

                }
                sb.Append("o[++P");
                while (roadBlocks > 0)
                {
                    sb.Append("C");
                    roadBlocks--;
                }
                sb.Append("]");
                sb.Append("]");

                // next?

            }
            else if (UnityEngine.Random.Range(0, 100) <= 100)
            {
                sb.Append("[");
            Loop:
                if (firstLoop)
                {
                    sb.Append("[");
                    firstLoop = false;
                }
                else
                {
                    sb.Append("o[");
                }

                ruleset["J"] = rulesetI + "p[++O" + rulesetG + "]PJ";


                sb.Append("--PJ");

                sb.Append("]O" + rulesetG);

                while (index > 0)
                {

                    index--;
                    goto Loop;
                }
                sb.Append("o[--P");
                while (roadBlocks > 0)
                {
                    sb.Append("C");
                    roadBlocks--;
                }
                sb.Append("]");
                sb.Append("]");

                // next?
            }

            return sb;
        }

        StringBuilder RoundaboutAxiom(StringBuilder sb)
        {
            sb = new StringBuilder();
            string plusorminusA = ""; 
            string plusorminusB = "";
            string plusorminusC = "";
            string plusorminusD = "";

            plusorminusA = RandomTurnGenerator(plusorminusA);
            plusorminusB = RandomTurnGenerator(plusorminusB);
            plusorminusC = RandomTurnGenerator(plusorminusC);
            plusorminusD = RandomTurnGenerator(plusorminusD);

            int randRoadLength = UnityEngine.Random.Range(0, 4);
            int randRoadLengthB = UnityEngine.Random.Range(0, 4);

            switch (randRoadLength)
            {
                case 0: roundaboutRuleset["C"] = "111111"; break;
                case 1: roundaboutRuleset["C"] = "11111111"; break;
                case 2: roundaboutRuleset["C"] = "1111111111"; break;
                case 3: roundaboutRuleset["C"] = "111111111111"; break;
            };
            switch (randRoadLengthB)
            {
                case 0: roundaboutRuleset["B"] = "111111"; break;
                case 1: roundaboutRuleset["B"] = "11111111"; break;
                case 2: roundaboutRuleset["B"] = "1111111111"; break;
                case 3: roundaboutRuleset["B"] = "111111111111"; break;
            }

           // roundaboutRuleset["A"] = "[E]++[D]++D--E" + plusorminus + "A";
            roundaboutRuleset["A"] = "[E]"+ plusorminusA + "[D]"+ plusorminusB + "D" + plusorminusC + "E" + plusorminusD + "A";

            roundaboutRuleset["D"] = "2p[P2p[" + plusorminusA + "OC[" + plusorminusB + "OC]C[" + plusorminusC + "OC]Co[" + plusorminusD + "OC]]]";
            sb.Append("AP");
            roundaboutRuleset["E"] = "Bo[" + plusorminusD + "OB[" + plusorminusC + "OB]Bo[" + plusorminusB + "OB]]OBo[" + plusorminusA + "OB]";
            return sb;
        }


        string RandomTurnGenerator(string plusorminus)
        {
            int randNum = UnityEngine.Random.Range(0, 2);
            switch (randNum)
            {
                case 0: plusorminus = "--"; break;
                case 1: plusorminus = "++"; break;
                case 2: plusorminus = "----"; break;
                case 3: plusorminus = "++++"; break;
            }
            return plusorminus;
        }
    }

  
}

