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
using SpiritMod.Utilities;
using SpiritMod.Mechanics.QuestSystem;
using SpiritMod.Mechanics.QuestSystem.Quests;
using Terraria.UI.Chat;

namespace SpiritMod.UI.QuestUI
{
    public class QuestBookUI : UIState
    {
		public const float SELECTED_OPACITY = 0.25f;
		public const float HOVERED_OPACITY = 0.1f;

		private int _questSectionIndex;
        private int _questFilterIndex;
        private int _selectedQuestIndex;
		
		public Quest SelectedQuest { get; private set; }

		private UISelectableQuest[] _allQuestButtons;
		private bool[] _isPossible;
		private int _currentMaxPossible;

        private UIQuestBookButtonTextPanel[] _questSectionButtons;
        private UIQuestBookButtonTextPanel[] _questFilterButtons;
        private UIModifiedList _questList;
		private UISimpleWrappableText _questProgressCounterText;
		private UISimpleWrappableText _questTitleText;
		private UISimpleWrappableText _questCategoryText;
		private UISimpleWrappableText _questObjectivesText;
        private UISimpleWrappableText _questClientText;
		private UISimpleWrappableText _questClientTitle;
		private UISimpleWrappableText _questRewardsTitle;
		private UISimpleWrappableText _questObjectivesTitle;
		private UIShaderImage _obnoxiousTutorialGlow;
		private UIShaderImage _questImage;
		private UISimpleWrappableText _interactionWarningText;
		private UISelectableOutlineRectPanel _questInteractButton;
		private UISimpleWrappableText _questInteractText;
		private List<UISolid> _rightPageLines;
		private UIGridList _questRewardList;

		private bool _showingWarning;
		private Texture2D[] _imageMasks;

		public override void OnInitialize()
        {
			_isPossible = new bool[QuestManager.Quests.Count];
			_rightPageLines = new List<UISolid>();
			_imageMasks = new Texture2D[15];
			for (int i = 0; i < _imageMasks.Length; i++)
			{
				_imageMasks[i] = SpiritMod.Instance.GetTexture("UI/QuestUI/Textures/ImageMask" + i);
			}

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
            rightPage.Height.Set(510f, 0f);
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
					ChangeFilter(0);
					ChangeSection(index);
				};
                leftPage.Append(_questSectionButtons[i]);
            }
            ButtonArraySelect(_questSectionIndex, _questSectionButtons);
            #endregion

            leftPage.Append(CreateLine(22f, false));

            // bottom buttons
            #region bottom buttons
            _questFilterIndex = 0;
            _questFilterButtons = CreateButtons(26f, 0.7f, false, "All", "Main", "Explorer", "Forager", "Slayer", "Other");
            for (int i = 0; i < _questFilterButtons.Length; i++)
            {
                int index = i;
                _questFilterButtons[i].OnMouseDown += (UIMouseEvent evt, UIElement el) =>
				{
					ChangeFilter(index);
				};
                leftPage.Append(_questFilterButtons[i]);
            }
            ButtonArraySelect(_questFilterIndex, _questFilterButtons);
            #endregion

            leftPage.Append(CreateLine(48f, false));

			UIElement questContainer = new UIElement();
			questContainer.Width.Set(0f, 1f);
			questContainer.Top.Set(52f, 0f);
			questContainer.Height.Set(-72f, 1f);
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
				_allQuestButtons[i] = new UISelectableQuest(QuestManager.Quests[i], this);

				QuestManager.Quests[i].OnQuestStateChanged += _allQuestButtons[i].HandleState;
				QuestManager.Quests[i].OnQuestStateChanged += (Quest q) => FullBookUpdate();

				_allQuestButtons[i].HandleState(QuestManager.Quests[i]);
			}
			for (int i = 0; i < _allQuestButtons.Length; i++)
			{
				int index = i;
				_allQuestButtons[i].OnMouseDown += (UIMouseEvent evt, UIElement el) =>
				{
					_selectedQuestIndex = index;
					ButtonArraySelect(index, _allQuestButtons);
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

			_questProgressCounterText = new UISimpleWrappableText("", 0.8f);
			_questProgressCounterText.Top.Set(-16f, 1f);
			_questProgressCounterText.Left.Set(-50f, 1f);
			_questProgressCounterText.Width.Set(50f, 0f);
			_questProgressCounterText.Height.Set(16f, 0f);
			_questProgressCounterText.Centered = true;
			_questProgressCounterText.Colour = new Color(43, 28, 17);
			leftPage.Append(_questProgressCounterText);

			// quest title
			_questTitleText = new UISimpleWrappableText("", 0.8f);
			_questTitleText.Top.Set(-26f, 0f);
			_questTitleText.Width.Set(0f, 1f);
			_questTitleText.Large = true;
			_questTitleText.Centered = true;
			_questTitleText.Colour = new Color(43, 28, 17);
            rightPage.Append(_questTitleText);

            // quest category
            _questCategoryText = new UISimpleWrappableText("", 1.08f);
			_questCategoryText.Top.Set(21f, 0f);
			_questCategoryText.Width.Set(0f, 1f);
			_questCategoryText.Centered = true;
			_questCategoryText.Border = true;
			_questCategoryText.BorderColour = new Color(43, 28, 17) * 0.8f;
			rightPage.Append(_questCategoryText);

			// image
			_questImage = new UIShaderImage(null);
			_questImage.Effect = SpiritMod.Instance.GetEffect("Effects/QuestShaders");
			_questImage.Pass = _questImage.Effect.CurrentTechnique.Passes["Sepia"];
			_questImage.PointSample = true;
			_questImage.Top.Set(44f, 0f);
			_questImage.Width.Set(0f, 1f);
			_questImage.Height.Set(130f, 0f);
			rightPage.Append(_questImage);

			// objectives title
			_questObjectivesTitle = new UISimpleWrappableText("Objectives", 0.8f);
			_questObjectivesTitle.Top.Set(175f, 0f);
			_questObjectivesTitle.Colour = new Color(43, 28, 17);
            rightPage.Append(_questObjectivesTitle);

            rightPage.Append(CreateLine(189f));

			_questObjectivesText = new UISimpleWrappableText("", 0.7f);
			_questObjectivesText.Top.Set(195f, 0f);
			_questObjectivesText.Colour = new Color(43, 28, 17);
            rightPage.Append(_questObjectivesText);

			// client title
			_questClientTitle = new UISimpleWrappableText("Client - ", 0.8f);
			_questClientTitle.Top.Set(254f, 0f);
			_questClientTitle.Colour = new Color(43, 28, 17);
            rightPage.Append(_questClientTitle);

            rightPage.Append(CreateLine(268f));

            _questClientText = new UISimpleWrappableText("", 0.7f, false, true);
			_questClientText.Top.Set(272f, 0f);
			_questClientText.Width.Set(0f, 1f);
			_questClientText.MinWidth.Set(0f, 1f);
			_questClientText.Colour = new Color(43, 28, 17);
            rightPage.Append(_questClientText);

            // rewards title
            _questRewardsTitle = new UISimpleWrappableText("Rewards", 0.8f);
			_questRewardsTitle.Top.Set(392f, 0f);
			_questRewardsTitle.Colour = new Color(43, 28, 17);
            rightPage.Append(_questRewardsTitle);

            rightPage.Append(CreateLine(406f));

			_questRewardList = new UIGridList();
			_questRewardList.Top.Set(410, 0f);
			_questRewardList.Height.Set(44f, 0f);
			_questRewardList.Width.Set(0f, 1f);
			_questRewardList.ItemSize = new Vector2(42);
			_questRewardList.ListPadding = 4f;
			rightPage.Append(_questRewardList);

			_interactionWarningText = new UISimpleWrappableText("", 0.65f);
			_interactionWarningText.Top.Set(459f, 0f);
			_interactionWarningText.Left.Set(0f, 0f);
			_interactionWarningText.Height.Set(22f, 0f);
			_interactionWarningText.Width.Set(-106f, 1f);
			_interactionWarningText.UseChatManager = true;
			_interactionWarningText.Colour = new Color(43, 28, 17);
			//_interactionWarningText.Centered = true;
			rightPage.Append(_interactionWarningText);

			_questInteractButton = new UISelectableOutlineRectPanel();
			_questInteractButton.Top.Set(455f, 0f);
			_questInteractButton.Left.Set(-110f, 1f);
			_questInteractButton.Height.Set(22f, 0f);
			_questInteractButton.Width.Set(110f, 0f);
			_questInteractButton.DrawBorder = true;
			_questInteractButton.SelectedFillColour = new Color(102, 86, 67) * SELECTED_OPACITY;
			_questInteractButton.HoverFillColour = new Color(102, 86, 67) * HOVERED_OPACITY;
			_questInteractButton.NormalOutlineColour = new Color(102, 86, 67) * 0.5f;
			_questInteractButton.SelectedOutlineColour = new Color(102, 86, 67) * 0.9f;
			_questInteractButton.HoverOutlineColour = new Color(102, 86, 67) * 0.7f;
			_questInteractButton.OnMouseDown += (UIMouseEvent evt, UIElement listeningElement) => 
			{
				_questInteractButton.IsSelected = true;
			};
			_questInteractButton.OnMouseUp += (UIMouseEvent evt, UIElement listeningElement) =>
			{
				_questInteractButton.IsSelected = false;
			};
			_questInteractButton.OnClick += (UIMouseEvent evt, UIElement listeningElement) =>
			{
				if (SelectedQuest.RewardsGiven) return;

				if (SelectedQuest.IsCompleted)
				{
					QuestManager.GiveRewards(SelectedQuest);
					UpdateCurrentQuest();
				}
				else if (!SelectedQuest.IsActive)
				{
					bool success = QuestManager.ActivateQuest(_selectedQuestIndex);
					if (success)
					{
						ChangeSection(1); // change section to active section
						UpdateCurrentQuest();
					}
					else
					{
						if (QuestManager.ActiveQuests.Count >= QuestManager.MAX_QUESTS_ACTIVE)
						{
							_interactionWarningText.Text = "You cannot activate any more quests.";
						}
					}
				}
				else
				{
					if (_showingWarning)
					{
						QuestManager.DeactivateQuest(_selectedQuestIndex);
						_interactionWarningText.Text = "";
						_showingWarning = false;
						return;
					}

					// show a warning
					_interactionWarningText.Text = "Are you sure? You will [c/910000:lose your progress].";
					_showingWarning = true;
				}
			};

			_questInteractText = new UISimpleWrappableText("Activate");
			_questInteractText.Centered = true;
			_questInteractText.Top.Set(-9f, 0f);
			_questInteractText.Width.Set(0f, 1f);
			_questInteractText.Height.Set(0f, 1f);
			_questInteractText.Scale = 0.8f;
			_questInteractText.UpdateText();
			_questInteractText.Colour = new Color(43, 28, 17);
			_questInteractButton.Append(_questInteractText);
			rightPage.Append(_questInteractButton);

			_obnoxiousTutorialGlow = new UIShaderImage(Main.blackTileTexture);
			_obnoxiousTutorialGlow.Top.Set(414f, 0f);
			_obnoxiousTutorialGlow.Left.Set(-150f, 1f);
			_obnoxiousTutorialGlow.Height.Set(102f, 0f);
			_obnoxiousTutorialGlow.Width.Set(189f, 0f);
			_obnoxiousTutorialGlow.Effect = SpiritMod.Instance.GetEffect("Effects/QuestShaders");
			_obnoxiousTutorialGlow.Effect.Parameters["NoiseTexture"].SetValue(SpiritMod.Instance.GetTexture("Noise/noise"));
			_obnoxiousTutorialGlow.Effect.Parameters["NoiseWidth"].SetValue(489f);
			_obnoxiousTutorialGlow.Pass = _obnoxiousTutorialGlow.Effect.CurrentTechnique.Passes["GodRays"];
			_obnoxiousTutorialGlow.PreDraw += () =>
			{
				var e = _obnoxiousTutorialGlow.Effect;
				float radius = 40f;
				Vector2 size = new Vector2(190f, 102f);
				e.Parameters["GodRayRadius"].SetValue(radius);
				e.Parameters["GodRayRadiusTimesPi"].SetValue(radius * MathHelper.Pi);
				e.Parameters["GodRayMovementMultiplier"].SetValue(0.25f);
				e.Parameters["GodRayRadiusTimesPiOverTwo"].SetValue(radius * MathHelper.PiOver2);
				e.Parameters["RectSize"].SetValue(size);
				e.Parameters["InnerRectSize"].SetValue(new Vector2(size.X - radius * 2f, size.Y - radius * 2f));
				e.Parameters["RectCenter"].SetValue(size * 0.5f);
				e.Parameters["Time"].SetValue((float)(Main._drawInterfaceGameTime.TotalGameTime.TotalSeconds));
				e.Parameters["UVRectMinX"].SetValue(radius / size.X);
				e.Parameters["UVRectMinY"].SetValue(radius / size.Y);
				e.Parameters["UVRectMaxX"].SetValue((size.X - radius) / size.X);
				e.Parameters["UVRectMaxY"].SetValue((size.Y - radius) / size.Y);
				e.Parameters["GodRayColour"].SetValue(new Color(255, 243, 178, 255).ToVector4());
			};
			rightPage.Append(_obnoxiousTutorialGlow);

			mainWindow.Append(leftPage);
            mainWindow.Append(rightPage);

			UpdateList();

			SelectQuest(_allQuestButtons[_selectedQuestIndex].MyQuest);

			Append(mainWindow);
        }

		private void FullBookUpdate()
		{
			if (!(SpiritMod.Instance.BookUserInterface.CurrentState is QuestBookUI)) return;

			ButtonArraySelect(_questSectionIndex, _questSectionButtons);
			ButtonArraySelect(_questFilterIndex, _questFilterButtons);
			ButtonArraySelect(_selectedQuestIndex, _allQuestButtons);
			UpdateList();
			UpdateCurrentQuest();
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
				button.DrawFilled = true;
				button.SelectedFillColour = new Color(102, 86, 67) * SELECTED_OPACITY;
				button.HoverFillColour = new Color(102, 86, 67) * HOVERED_OPACITY;
				button.Top.Set(y, 0f);
                button.Left.Set(0f, totalPrec);
                button.Width.Set(0f, widths[i]);
                button.Height.Set(18f, 0f);
                array[i] = button;
                totalPrec += widths[i];
            }

            return array;
        }

		private void UpdateCurrentQuest()
		{
			if (SelectedQuest == null) return;

			if (SelectedQuest.IsCompleted) ChangeSection(2);
			else if (SelectedQuest.IsActive) ChangeSection(1);
			else ChangeSection(0);

			SelectQuest(SelectedQuest, false);
		}

		public void SelectQuest(Quest quest, bool selectOnLeftPage = true)
		{
			// open book if not open.
			if (!(SpiritMod.Instance.BookUserInterface.CurrentState is QuestBookUI))
			{
				QuestManager.SetBookState(true);
			}

			// this is here for external quest book openings.
			if (selectOnLeftPage)
			{
				_questSectionIndex = quest.IsCompleted ? 2 : (quest.IsActive ? 1 : 0);
				_questFilterIndex = 0;
				for (int i = 0; i < _allQuestButtons.Length; i++)
				{
					if (_allQuestButtons[i].MyQuest == quest)
					{
						_selectedQuestIndex = i;
						break;
					}
				}

				UpdateList();

				_questList.Goto((UIElement el) => (el is UISelectableQuest q) && q.MyQuest == quest);

				ButtonArraySelect(_questSectionIndex, _questSectionButtons);
				ButtonArraySelect(_questFilterIndex, _questFilterButtons);
				ButtonArraySelect(_selectedQuestIndex, _allQuestButtons);
			}

			_interactionWarningText.Text = "";
			_showingWarning = false;

			// show the undiscovered page.
			if (!quest.IsUnlocked)
			{
				_questTitleText.Scale = 0.47f;
				_questTitleText.Text = "This quest hasn't been discovered.";
				_questTitleText.Top.Set(-14f, 0.5f);

				_questImage.Texture = null;
				_questClientText.Text = "";
				_questCategoryText.Text = "";
				_questObjectivesText.Text = "";
				_questClientTitle.Text = "";
				_questObjectivesTitle.Text = "";
				_questRewardsTitle.Text = "";
				_questRewardList.Clear();
				_obnoxiousTutorialGlow.Texture = null;
				// just move this off screen
				_questInteractButton.Left.Set(-1000000f, 0f);

				foreach (UISolid solid in _rightPageLines)
				{
					solid.Color = Color.Transparent;
				}
				return;
			}

			foreach (UISolid solid in _rightPageLines)
			{
				solid.Color = new Color(102, 86, 67);
			}

			if (quest.RewardsGiven)
			{
				// just move this off screen
				_questInteractButton.Left.Set(-1000000f, 0f);
			}
			else
			{
				// bring the interact button back to the screen if it's gone
				_questInteractButton.Left.Set(-110f, 1f);
			}

			_obnoxiousTutorialGlow.Texture = (quest.TutorialActivateButton && !quest.IsActive && !quest.RewardsGiven) ? Main.blackTileTexture : null;
			_questTitleText.Top.Set(-8f, 0f);
			_questTitleText.Scale = quest.QuestTitleScale;
			_questTitleText.Text = quest.QuestName;
			_questObjectivesTitle.Text = "Objectives";
			_questRewardsTitle.Text = "Rewards";
			_questImage.Texture = quest.QuestImage;
			_questInteractText.Text = quest.IsCompleted ? "Claim rewards!" : (quest.IsActive ? "Deactivate" : "Activate");
			_questClientTitle.Text = "Client - " + quest.QuestClient;
			_questClientText.Text = quest.QuestDescription;
			_questClientText.UpdateText();
			(Color, string) category = QuestUtils.GetCategoryInfo(quest.QuestType);
			_questCategoryText.Text = category.Item2;
			_questCategoryText.Colour = category.Item1;
			_questObjectivesText.Text = quest.GetObjectives(quest.IsActive);

			_questRewardList.Clear();
			if (quest.QuestRewards != null)
			{
				foreach (var reward in quest.QuestRewards)
				{
					_questRewardList.Add(new UIRewardItem(reward.Item1, reward.Item2));
				}
			}

			// pick a "random" mask
			int maskIndex = (quest.QuestName.Length * quest.QuestDescription.Length) % _imageMasks.Length;
			_questImage.Effect.Parameters["AlphaMaskTexture"].SetValue(_imageMasks[maskIndex]);

			SelectedQuest = quest;
		}

		private void ChangeSection(int newSection)
		{
			_questSectionIndex = newSection;
			ButtonArraySelect(_questSectionIndex, _questSectionButtons);
			UpdateList();
		}

		private void ChangeFilter(int newFilter)
		{
			_questFilterIndex = newFilter;
			ButtonArraySelect(_questFilterIndex, _questFilterButtons);
			UpdateList();
		}

		private UISolid CreateLine(float y, bool rightPage = true)
        {
            UISolid solid = new UISolid();
            solid.Left.Set(0f, 0f);
            solid.Top.Set(y, 0f);
            solid.Width.Set(0f, 1f);
            solid.Height.Set(1f, 0f);
            solid.Color = new Color(102, 86, 67);
			if (rightPage) _rightPageLines.Add(solid);
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

			IEnumerable<UISelectableQuest> orderedFilteredQuests = _allQuestButtons.ToList().Where(q => _isPossible[q.MyQuest.WhoAmI]);

			// filter by section
			switch (_questSectionIndex)
			{
				case 0:
					// get all inactive quests
					orderedFilteredQuests = orderedFilteredQuests.Where(q => !q.MyQuest.IsActive && !q.MyQuest.IsCompleted);
					_questProgressCounterText.Text = "";
					break;
				case 1:
					// get all active quests
					orderedFilteredQuests = orderedFilteredQuests.Where(q => q.MyQuest.IsActive);
					_questProgressCounterText.Text = "(" + orderedFilteredQuests.Count() + "/" + QuestManager.MAX_QUESTS_ACTIVE + ")";
					break;
				case 2:
					// get all completed quests
					orderedFilteredQuests = orderedFilteredQuests.Where(q => q.MyQuest.IsCompleted);
					_questProgressCounterText.Text = "(" + orderedFilteredQuests.Count() + "/" + _currentMaxPossible + ")";
					break;
			}

			// filter by quest type
			if (_questFilterIndex > 0)
			{
				int typeFilter = (int)QuestType.Other | (int)QuestType.Designer;
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
					.OrderBy(q => (q.MyQuest.IsUnlocked ? -100 : 0) + q.MyQuest.Difficulty);
			}
			else
			{
				orderedFilteredQuests = orderedFilteredQuests
					.OrderBy(q => (q.MyQuest.IsUnlocked ? -10000000 : 0) + ((int)QuestUtils.GetBaseQuestType(q.MyQuest.QuestType)) * 1000 + q.MyQuest.Difficulty);
			}

			foreach (var quest in orderedFilteredQuests)
			{
				_questList.Add(quest);
			}
		}

		private void CalculatePossible()
		{
			_currentMaxPossible = 0;
			for (int i = 0; i < QuestManager.Quests.Count; i++)
			{
				_isPossible[i] = QuestManager.Quests[i].IsQuestPossible();
				if (_isPossible[i]) _currentMaxPossible++;
			}
		}

		public override void OnActivate()
		{
			CalculatePossible();

			// update this text every time we activate just to ensure no wrapping issues occur.
			_questClientText?.UpdateText();

			SelectQuest(_allQuestButtons[_selectedQuestIndex].MyQuest, false);
			UpdateList();

			base.OnActivate();
		}

		public override void Update(GameTime gameTime)
		{
			if (SelectedQuest != null && SelectedQuest.IsActive)
			{
				_questObjectivesText.Text = SelectedQuest.GetObjectives(true);
			}
			base.Update(gameTime);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			var viewPort = Main.graphics.GraphicsDevice.Viewport;
			_questImage.Effect.Parameters["MATRIX"].SetValue(Matrix.CreateOrthographicOffCenter(0, viewPort.Width, viewPort.Height, 0, 0, -1));

			base.Draw(spriteBatch);
		}
	}
}
