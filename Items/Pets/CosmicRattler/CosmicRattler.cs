using SpiritMod.Buffs.Pet;
using SpiritMod.Projectiles.Pet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Pets.CosmicRattler
{
	public class CosmicRattler : ModItem
	{
		public override bool Autoload(ref string name) => false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cosmic Rattler");
			Tooltip.SetDefault("Summons a Starachnid companion");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.Fish);
			item.shoot = ModContent.ProjectileType<CosmicRattlerPet>();
			item.buffType = ModContent.BuffType<CosmicRattlerPetBuff>();
			item.UseSound = SoundID.NPCDeath6;
			item.rare = ItemRarityID.LightRed;
		}

		public override void UseStyle(Player player)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
				player.AddBuff(item.buffType, 3600, true);
		}

		public override bool CanUseItem(Player player) => player.miscEquips[0].IsAir;
	}
}