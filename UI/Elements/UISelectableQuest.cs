using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.UI.QuestUI;
using Terraria.UI;
using SpiritMod.NPCs.Town.QuestSystem;

namespace SpiritMod.UI.Elements
{
    public class UISelectableQuest : UISelectableOutlineRectPanel
	{
        public Quest MyQuest { get; protected set; }

		public UISelectableQuest(Quest quest)
		{
			MyQuest = quest;
		}
    }
}
