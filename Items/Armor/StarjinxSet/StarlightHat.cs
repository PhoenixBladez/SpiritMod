using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SpiritMod.Items.Armor.StarjinxSet
{
	[AutoloadEquip(EquipType.Head)]
    public class StarlightHat : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod) => false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starlight Hat");
			Tooltip.SetDefault("12% increased magic damage and 6% increased magic critical strike chance");
			SpiritGlowmask.AddGlowMask(Item.type, Texture + "_glow");
		}
		public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 24;
            Item.value = Item.sellPrice(gold : 8);
            Item.rare = ItemRarityID.Pink;
            Item.defense = 7;
		}

		public override void DrawHair(ref bool drawHair, ref bool drawAltHair) => drawHair = drawAltHair = false;
		public override bool DrawHead() => false;

		public override void UpdateEquip(Player player)
		{
			player.magicDamage += 0.12f;
			player.magicCrit += 6;
		}
		public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor) => glowMaskColor = Color.White * 0.75f;
		public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == Mod.Find<ModItem>("StarlightMantle").Type && legs.type == Mod.Find<ModItem>("StarlightSandals").Type;

		public override void UpdateArmorSet(Player player)
        {
			player.setBonus = ("Greatly increases mana useage and prevents useage of mana potions\n"
					  + "Running out of mana produces a manajinx pylon near you\n"
					  + "Collecting the manajinx pylon restores all mana and temporarily enchants magic weapons with stars");
			player.manaCost *= 1.5f;
			MyPlayer modplayer = (MyPlayer)player.GetModPlayer(Mod, "MyPlayer");
			modplayer.StarjinxSet = true;
			if (Main.rand.Next(30) == 0)
		    {
				int gore = Gore.NewGore(player.position + new Vector2(Main.rand.Next(player.width), Main.rand.Next(player.height)), 
					player.velocity / 2 + Main.rand.NextVector2Circular(1, 1), 
					Mod.Find<ModGore>("Gores/StarjinxGore").Type, 
					Main.rand.NextFloat(0.25f, 0.75f));
				Main.playerDrawGore.Add(gore);
			}
		}
		public override void ArmorSetShadows(Player player) => player.armorEffectDrawOutlinesForbidden = true;
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(Mod, "Starjinx", 8);
			recipe.AddIngredient(ItemID.Silk, 4);
			recipe.AddIngredient(ItemID.FallenStar, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
    }
}
