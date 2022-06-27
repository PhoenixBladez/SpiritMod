using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using SpiritMod.Utilities;

namespace SpiritMod.Mechanics.AutoReuseTooltip
{
	public class Auto_Reuse_Tooltip : GlobalItem
	{
		public override bool InstancePerEntity
		{
			get
			{
				return true;
			}
		}

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			var config = ModContent.GetInstance<SpiritClientConfig>();
			if (config.AutoReuse && item.ammo == 0 && !item.accessory)
			{
				if ((item.autoReuse || item.channel) && item.ranged)
				{
					TooltipLine line = new TooltipLine(Mod, "isAutoreused", "Autofire: [c/64FF64:✔]");
					tooltips.Add(line);
				}
				else if (!item.autoReuse && item.ranged)
				{
					TooltipLine line2 = new TooltipLine(Mod, "isntAutoreused", "Autofire: [c/FF505A:✘]");
					tooltips.Add(line2);
				}	

				if ((item.autoReuse || item.channel) && item.melee)
				{
					TooltipLine line = new TooltipLine(Mod, "isAutoreused", "Autoswing: [c/64FF64:✔]");
					tooltips.Add(line);
				}
				else if (!item.autoReuse && item.melee)
				{
					TooltipLine line2 = new TooltipLine(Mod, "isntAutoreused", "Autoswing: [c/FF505A:✘]");
					tooltips.Add(line2);
				}	

				if ((item.autoReuse || item.channel) && item.magic)
				{
					TooltipLine line = new TooltipLine(Mod, "isAutoreused", "Autofire: [c/64FF64:✔]");
					tooltips.Add(line);
				}
				else if (!item.autoReuse && item.magic)
				{
					TooltipLine line2 = new TooltipLine(Mod, "isntAutoreused", "Autofire: [c/FF505A:✘]");
					tooltips.Add(line2);
				}	

				if ((item.autoReuse || item.channel) && item.summon)
				{
					TooltipLine line = new TooltipLine(Mod, "isAutoreused", "Autofire: [c/64FF64:✔]");
					tooltips.Add(line);
				}
				else if (!item.autoReuse && item.summon)
				{
					TooltipLine line2 = new TooltipLine(Mod, "isntAutoreused", "Autofire: [c/FF505A:✘]");
					tooltips.Add(line2);
				}
			}
		}
	}
}
