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

using SpiritMod.Mechanics.QuestSystem;
using SpiritMod.Mechanics.QuestSystem.Quests;

namespace SpiritMod.UI
{
    public class QuestBookUI : UIState
    {
        private int _questSectionIndex;
        private int _questFilterIndex;
        private int _selectedQuestIndex;

		private UISelectableQuest[] _allQuestButtons;
        private UIQuestBookButtonTextPanel[] _questSectionButtons;
        private UIQuestBookButtonTextPanel[] _questFilterButtons;
        private UIModifiedList _questList;
		private UISimpleWrappableText _questTitleText;
		private UISimpleWrappableText _questCategoryText;
		private UISimpleWrappableText _questObjectivesText;
        private UISimpleWrappableText _questClientText;
		private UISimpleWrappableText _questClientTitle;

		public override void OnInitialize()
        {
            UIMoveExpandWindow mainWindow = new UIMoveExpandWindow(SpiritMod.Instance.GetTexture("UI/QuestUI/Textures/AdventurerBook"), false, false, 10);

            mainWindow.Left.Set(450, 0);
            mainWindow.Top.Set(230, 0);
            mainWindow.Width.Set(1019, 0);
            mainWindow.Height.Set(706, 0);
			mainWindow.MinWidth.Set(1019, 0);
			mainWindow.MinHeight.Set(706, 0);
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
					UpdateList();
				};
                leftPage.Append(_questSectionButtons[i]);
            }
            ButtonArraySelect(_questSectionIndex, _questSectionButtons);
            #endregion

            leftPage.Append(CreateLine(22f));

            // bottom buttons
            #region bottom buttons
            _questFilterIndex = 0;
            _questFilterButtons = CreateButtons(26f, 0.7f, false, "All", "Main", "Explorer", "Forager", "Slayer", "Other");
            for (int i = 0; i < _questFilterButtons.Length; i++)
            {
                int index = i;
                _questFilterButtons[i].OnMouseDown += (UIMouseEvent evt, UIElement el) =>
                {
                    _questFilterIndex = index;
                    ButtonArraySelect(index, _questFilterButtons);
					// refresh
					UpdateList();
                };
                leftPage.Append(_questFilterButtons[i]);
            }
            ButtonArraySelect(_questFilterIndex, _questFilterButtons);
            #endregion

            leftPage.Append(CreateLine(48f));

			UIElement questContainer = new UIElement();
			questContainer.Width.Set(0f, 1f);
			questContainer.Top.Set(52f, 0f);
			questContainer.Height.Set(-52f, 1f);
			questContainer.SetPadding(0f);

            _questList = new UIModifiedList();
            _questList.Width.Set(-18f, 1f);
            _questList.Height.Set(0f, 1f);
            _questList.SetPadding(0f);
            _questList.ListPadding = 2f;

			// create all the quest panels
			#region quests
			_allQuestButtons = new UISelectableQuest[QuestManager.Quests.Count];
			for (int i = 0; i < QuestManager.Quests.Count; i++)
			{
				_allQuestButtons[i] = QuestUtils.QuestAsPanel(QuestManager.Quests[i]);
				QuestManager.Quests[i].OnQuestUnlockChanged += _allQuestButtons[i].HandleUnlock;
			}
			for (int i = 0; i < _allQuestButtons.Length; i++)
			{
				int index = i;
				_allQuestButtons[i].OnMouseDown += (UIMouseEvent evt, UIElement el) =>
				{
					_selectedQuestIndex = index;
					ButtonArraySelect(index, _allQuestButtons);

					SelectQuest((el as UISelectableQuest).MyQuest);
				};
			}
			ButtonArraySelect(_selectedQuestIndex, _allQuestButtons);
			#endregion

			UIQuestBookScrollBar questListScrollbar = new UIQuestBookScrollBar(SpiritMod.Instance.BookUserInterface);
			questListScrollbar.SetView(100f, 1000f);
			questListScrollbar.Height.Set(-2f, 1f);
			questListScrollbar.Top.Set(2f, 0f);
			questListScrollbar.HAlign = 1f;
			questListScrollbar.Colour = new Color(43, 28, 17);

			_questList.SetScrollbar(questListScrollbar);

			questContainer.Append(questListScrollbar);
			questContainer.Append(_questList);
			leftPage.Append(questContainer);

			// quest title
			_questTitleText = new UISimpleWrappableText("", 0.8f);
			_questTitleText.Top.Set(-8f, 0f);
			_questTitleText.Width.Set(0f, 1f);
			_questTitleText.Large = true;
			_questTitleText.Centered = true;
			_questTitleText.Colour = new Color(43, 28, 17);
            rightPage.Append(_questTitleText);

            // quest category
            _questCategoryText = new UISimpleWrappableText("", 0.8f);
			_questCategoryText.Top.Set(26f, 0f);
			_questCategoryText.Width.Set(0f, 1f);
			_questCategoryText.Centered = true;
			_questCategoryText.Scale = 1.08f;
			_questCategoryText.Border = true;
			_questCategoryText.BorderColour = new Color(43, 28, 17) * 0.8f;
			rightPage.Append(_questCategoryText);

            // objectives title
            UISimpleWrappableText objectivesTitle = new UISimpleWrappableText("Objectives", 0.8f);
            objectivesTitle.Top.Set(187f, 0f);
            objectivesTitle.Colour = new Color(43, 28, 17);
            rightPage.Append(objectivesTitle);

            rightPage.Append(CreateLine(201f));

			_questObjectivesText = new UISimpleWrappableText("", 0.7f);
			_questObjectivesText.Top.Set(205f, 0f);
			_questObjectivesText.Colour = new Color(43, 28, 17);
            rightPage.Append(_questObjectivesText);

			// client title
			_questClientTitle = new UISimpleWrappableText("Client - ", 0.8f);
			_questClientTitle.Top.Set(266f, 0f);
			_questClientTitle.Colour = new Color(43, 28, 17);
            rightPage.Append(_questClientTitle);

            rightPage.Append(CreateLine(280f));

            _questClientText = new UISimpleWrappableText("", 0.7f, false, true);
			_questClientText.Top.Set(284f, 0f);
			_questClientText.Width.Set(0f, 1f);
			_questClientText.MinWidth.Set(0f, 1f);
			_questClientText.Colour = new Color(43, 28, 17);
            rightPage.Append(_questClientText);

            // rewards title
            UISimpleWrappableText rewardsTitle = new UISimpleWrappableText("Rewards", 0.8f);
            rewardsTitle.Top.Set(404f, 0f);
            rewardsTitle.Colour = new Color(43, 28, 17);
            rightPage.Append(rewardsTitle);

            rightPage.Append(CreateLine(418f));

            mainWindow.Append(leftPage);
            mainWindow.Append(rightPage);

			UpdateList();

			SelectQuest(_allQuestButtons[_selectedQuestIndex].MyQuest);

			Append(mainWindow);
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

		public void SelectQuest(Quest quest)
		{
			_questTitleText.Scale = quest.QuestTitleScale;
			_questTitleText.Text = quest.QuestName;
			_questClientTitle.Text = "Client - " + quest.QuestClient;
			_questClientText.Text = quest.QuestDescription;
			_questClientText.UpdateText();
			(Color, string) category = QuestUtils.GetCategoryInfo(quest.QuestType);
			_questCategoryText.Text = category.Item2;
			_questCategoryText.Colour = category.Item1;
			_questObjectivesText.Text = quest.GetObjectives(false);
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

        private void ButtonArraySelect(int selectIndex, UISelectableOutlineRectPanel[] buttons)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].IsSelected = (i == selectIndex);
            }
        }

		private void UpdateList()
		{
			_questList.Clear();

			IEnumerable<UISelectableQuest> orderedFilteredQuests = _allQuestButtons.ToList();

			// filter by section
			switch (_questSectionIndex)
			{
				case 0:
					// get all inactive quests
					orderedFilteredQuests = orderedFilteredQuests.Where(q => !q.MyQuest.QuestActive);
					break;
				case 1:
					// get all active quests
					orderedFilteredQuests = orderedFilteredQuests.Where(q => q.MyQuest.QuestActive);
					break;
				case 2:
					// get all completed quests
					orderedFilteredQuests = orderedFilteredQuests.Where(q => q.MyQuest.QuestCompleted);
					break;
			}

			// filter by quest type
			if (_questFilterIndex > 0)
			{
				int typeFilter = (int)QuestType.Other;
				switch (_questFilterIndex)
				{
					case 1:
						// get all Main quests
						typeFilter = (int)QuestType.Main;
						break;
					case 2:
						// get all Explorer quests
						typeFilter = (int)QuestType.Explorer;
						break;
					case 3:
						// get all Forager quests
						typeFilter = (int)QuestType.Forager;
						break;
					case 4:
						// get all Slayer quests
						typeFilter = (int)QuestType.Slayer;
						break;
				}
				orderedFilteredQuests = orderedFilteredQuests
					.Where(q => ((int)q.MyQuest.QuestType & typeFilter) != 0)
					.OrderBy(q => (q.MyQuest.QuestUnlocked ? -100 : 0) + q.MyQuest.Difficulty);
			}
			else
			{
				orderedFilteredQuests = orderedFilteredQuests
					.OrderBy(q => (q.MyQuest.QuestUnlocked ? -10000000 : 0) + ((int)QuestUtils.GetBaseQuestType(q.MyQuest.QuestType)) * 1000 + q.MyQuest.Difficulty);
			}

			foreach (var quest in orderedFilteredQuests)
			{
				SpiritMod.Instance.Logger.Debug(quest.MyQuest.QuestName);
				_questList.Add(quest);
			}
		}

		public override void OnActivate()
		{
			// update this text every time we activate just to ensure no wrapping issues occur.
			_questClientText?.UpdateText();

			base.OnActivate();
		}
	}
}
