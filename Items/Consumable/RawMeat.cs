
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
	public class RawMeat : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Raw Meat");
		}


		public override void SetDefaults()
		{
			Item.width = Item.height = 24;
			Item.rare = ItemRarityID.White;
			Item.maxStack = 1;

			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.useTime = Item.useAnimation = 20;

			Item.noMelee = true;
			Item.consumable = true;
			Item.autoReuse = false;

			Item.UseSound = SoundID.Item43;
		}

		public override bool OnPickup(Player player)
		{
			SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 2));
			{
				player.statLife += 10;
				player.HealEffect(10, true);
				player.AddBuff(BuffID.WellFed, 540);
			}
			return false;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(189, 191, 174, 100);
		}
		public override bool ItemSpace(Player player)
		{
			return true;
		}
	}
}
