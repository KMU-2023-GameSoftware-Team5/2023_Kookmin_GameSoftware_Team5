using System.Collections;
using UnityEngine;

namespace GSC
{
    public class GSCExecuter
    {
        readonly GSCController m_controller;

        public GSCExecuter(GSCController controller) =>
            m_controller = controller;

        public IEnumerator Execute()
        {
            GSCScriptLine now;

            while ((now = m_controller.Next()) is not null)
            {
                switch (now.Command)
                {
                    case GSCCommand.Text:
                        m_controller.AddText(now.Args[0]);
                        break;

                    case GSCCommand.Goto:
                        m_controller.BranchTo(now.Args[0]);
                        break;

                    case GSCCommand.If:
                        m_controller.AddConditional(now.Args[0], now.Args[1]);
                        break;

                    case GSCCommand.Waitif:
                        yield return m_controller.BranchConditional();
                        break;

                    case GSCCommand.Call:
                        m_controller.InvokeCallback(now.Args[0]);
                        break;

                    case GSCCommand.Scene:
                        m_controller.LoadScene(now.Args[0]);
                        yield break;

                    case GSCCommand.Ifchance:
                        int chance = int.Parse(now.Args[0]);
                        int choiced = Random.Range(0, 100);

                        if (choiced < chance)
                            m_controller.BranchTo(now.Args[1]);

                        break;

                    case GSCCommand.Givegold:
                        int gold = int.Parse(now.Args[0]);
                        m_controller.Givegold(gold);
                        break;

                    case GSCCommand.Givechar:
                        m_controller.GiveCharacter(now.Args[0]);
                        break;

                    case GSCCommand.Giveitem:
                        m_controller.GiveItem(now.Args[0]);
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
