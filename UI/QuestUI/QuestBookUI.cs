using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

using SpiritMod.UI.Elements;
using SpiritMod.UI.QuestUI;

using SpiritMod.NPCs.Town.QuestSystem;
using SpiritMod.NPCs.Town.QuestSystem.Quests;

namespace SpiritMod.UI
{
    public class QuestBookUI : UIState
    {
        private int _questSectionIndex;
        private int _questFilterIndex;
        private int _selectedQuestIndex;

        private UIQuestBookButtonTextPanel[] _questSectionButtons;
        private UIQuestBookButtonTextPanel[] _questFilterButtons;
        private UIList _questList;
        private UISimpleWrappableText _questObjectivesText;
        private UISimpleWrappableText _questClientText;

        public override void OnInitialize()
        {
            UIMoveExpandWindow mainWindow = new UIMoveExpandWindow(SpiritMod.Instance.GetTexture("UI/QuestUI/Textures/AdventurerBook"), false, false, 10);

            mainWindow.Left.Set(400, 0);
            mainWindow.Top.Set(400, 0);
            mainWindow.Width.Set(1019, 0);
            mainWindow.Height.Set(706, 0);
            mainWindow.SetPadding(0f);
            // ensure the UI stays on screen when moved
            mainWindow.ForceScreenStick = true;

            // left page area
            UIElement leftPage = new UIElement();
            leftPage.Left.Set(204f, 0f);
            leftPage.Top.Set(135f, 0f);
            leftPage.Width.Set(340f, 0f);
            leftPage.Height.Set(450f, 0f);
            leftPage.SetPadding(0);

            // right page area
            UIElement rightPage = new UIElement();
            rightPage.Left.Set(605f, 0f);
            rightPage.Top.Set(120f, 0f);
            rightPage.Width.Set(340f, 0f);
            rightPage.Height.Set(470f, 0f);
            rightPage.SetPadding(0);

            // top buttons
            #region top buttons
            _questSectionIndex = 0;
            _questSectionButtons = CreateButtons(0f, 0.8f, true, "Available", "Active", "Completed");
            for (int i = 0; i < _questSectionButtons.Length; i++)
            {
                int index = i;
                _questSectionButtons[i].OnMouseDown += (UIMouseEvent evt, UIElement el) =>
                {
                    _questSectionIndex = index;
                    ButtonArraySelect(index, _questSectionButtons);
                    // refresh
                };
                leftPage.Append(_questSectionButtons[i]);
            }
            ButtonArraySelect(_questSectionIndex, _questSectionButtons);
            #endregion

            leftPage.Append(CreateLine(22f));

            // bottom buttons
            #region bottom buttons
            _questFilterIndex = 0;
            _questFilterButtons = CreateButtons(26f, 0.7f, false, "All", "Main", "Explorer", "Forager", "Hunter", "Other");
            for (int i = 0; i < _questFilterButtons.Length; i++)
            {
                int index = i;
                _questFilterButtons[i].OnMouseDown += (UIMouseEvent evt, UIElement el) =>
                {
                    _questFilterIndex = index;
                    ButtonArraySelect(index, _questFilterButtons);
                    // refresh
                };
                leftPage.Append(_questFilterButtons[i]);
            }
            ButtonArraySelect(_questFilterIndex, _questFilterButtons);
            #endregion

            leftPage.Append(CreateLine(48f));

            _questList = new UIList();
            _questList.Width.Set(0f, 1f);
            _questList.Top.Set(52f, 0f);
            _questList.Height.Set(0f, 1f);
            _questList.SetPadding(0f);
            _questList.ListPadding = 2f;

            _questList.Add(QuestUtils.QuestAsPanel(QuestManager.GetQuest<CacophonousCries>()));
            _questList.Add(QuestUtils.QuestAsPanel(QuestManager.GetQuest<CacophonousCries>()));
            _questList.Add(QuestUtils.QuestAsPanel(QuestManager.GetQuest<CacophonousCries>()));

            leftPage.Append(_questList);

            // quest title
            UISimpleWrappableText questTitle = new UISimpleWrappableText("", 0.8f);
            questTitle.Top.Set(-8f, 0f);
            questTitle.Width.Set(0f, 1f);
            questTitle.Large = true;
            questTitle.Centered = true;
            questTitle.Text = QuestManager.GetQuest<CacophonousCries>().QuestName;
            questTitle.Colour = new Color(43, 28, 17);
            rightPage.Append(questTitle);

            // quest category
            UISimpleWrappableText questCategory = new UISimpleWrappableText("", 0.8f);
            questCategory.Top.Set(30f, 0f);
            questCategory.Width.Set(0f, 1f);
            questCategory.Centered = true;
            questCategory.Text = QuestManager.GetQuest<CacophonousCries>().QuestType.ToString();
            questCategory.Colour = new Color(188, 38, 38);
            rightPage.Append(questCategory);

            // objectives title
            UISimpleWrappableText objectivesTitle = new UISimpleWrappableText("Objectives", 0.8f);
            objectivesTitle.Top.Set(147f, 0f);
            objectivesTitle.Colour = new Color(43, 28, 17);
            rightPage.Append(objectivesTitle);

            rightPage.Append(CreateLine(161f));

            UISimpleWrappableText objectivesText = new UISimpleWrappableText("", 0.7f);
            objectivesText.Top.Set(165f, 0f);
            objectivesText.Text = QuestManager.GetQuest<CacophonousCries>().GetObjectives(false);
            objectivesText.Colour = new Color(43, 28, 17);
            rightPage.Append(objectivesText);

            // client title
            UISimpleWrappableText clientTitle = new UISimpleWrappableText("Client - The Guide", 0.8f);
            clientTitle.Top.Set(226f, 0f);
            clientTitle.Colour = new Color(43, 28, 17);
            rightPage.Append(clientTitle);

            rightPage.Append(CreateLine(240f));

            UISimpleWrappableText clientText = new UISimpleWrappableText("", 0.7f, false, true);
            clientText.Top.Set(244f, 0f);
            clientText.Width.Set(0f, 1f);
            clientText.Colour = new Color(43, 28, 17);
            rightPage.Append(clientText);

            // rewards title
            UISimpleWrappableText rewardsTitle = new UISimpleWrappableText("Rewards", 0.8f);
            rewardsTitle.Top.Set(404f, 0f);
            rewardsTitle.Colour = new Color(43, 28, 17);
            rightPage.Append(rewardsTitle);

            rightPage.Append(CreateLine(418f));

            mainWindow.Append(leftPage);
            mainWindow.Append(rightPage);

            Append(mainWindow);

            clientText.Text = QuestManager.GetQuest<CacophonousCries>().QuestDescription;
        }

        private UIQuestBookButtonTextPanel[] CreateButtons(float y, float textScale, bool equalWidths, params string[] texts)
        {
            UIQuestBookButtonTextPanel[] array = new UIQuestBookButtonTextPanel[texts.Length];
            
            // calculate weighted widths
            float totalWidth = 0f;
            float[] widths = new float[texts.Length];
            for (int i = 0; i < texts.Length; i++)
            {
                widths[i] = Main.fontMouseText.MeasureString(texts[i]).X + 10;
                totalWidth += widths[i];
            }
            for (int i = 0; i < texts.Length; i++) 
            {
                if (!equalWidths)
                {
                    widths[i] /= totalWidth;
                }
                else
                {
                    widths[i] = 1f / texts.Length;
                }
            }

            // create the buttons
            float totalPrec = 0f;
            for (int i = 0; i < texts.Length; i++)
            {
                UIQuestBookButtonTextPanel button = new UIQuestBookButtonTextPanel(texts[i]);
                button.TextScale = textScale;
                button.Top.Set(y, 0f);
                button.Left.Set(0f, totalPrec);
                button.Width.Set(0f, widths[i]);
                button.Height.Set(18f, 0f);
                array[i] = button;
                totalPrec += widths[i];
            }

            return array;
        }

        private UISolid CreateLine(float y)
        {
            UISolid solid = new UISolid();
            solid.Left.Set(0f, 0f);
            solid.Top.Set(y, 0f);
            solid.Width.Set(0f, 1f);
            solid.Height.Set(1f, 0f);
            solid.Color = new Color(102, 86, 67);
            return solid;
        }

        private void ButtonArraySelect(int selectIndex, UIQuestBookButtonTextPanel[] buttons)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].IsSelected = (i == selectIndex);
            }
        }
    }
}
