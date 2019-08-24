using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.MagalaArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class MagalaHelm : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gore Magala Veil");
            Tooltip.SetDefault("Increases maximum health by 10 and maximum mana by 60\n ~Donator item~");

        }


        int timer = 0;

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 20;
            item.value = 3000;
            item.rare = 5;
            item.defense = 14;
        }
        public override void UpdateEquip(Player player)
        {
            player.statManaMax2 += 60;
            player.statLifeMax2 += 10;
        }

            public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Attacks inflict Frenzy Virus on foes \n Attacks cause the player to become imbued with a modified Virus";
            player.GetModPlayer<MyPlayer>(mod).magalaSet = true;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("MagalaPlate") && legs.type == mod.ItemType("MagalaLegs");
        }
		 public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "MagalaScale", 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }
    }
}
