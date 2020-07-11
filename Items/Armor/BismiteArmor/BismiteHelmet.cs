using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.BismiteArmor
{
	[AutoloadEquip(EquipType.Head)]
	public class BismiteHelmet : SpiritSetbonusHead
	{
		public override string SetDisplayName => "Bismite Helmet";
		public override string SetTooltip => "4% increased movement speed";
		public override bool SetBody(Item body) => body.type == ModContent.ItemType<BismiteChestplate>();
		public override bool SetLegs(Item legs) => legs.type == ModContent.ItemType<BismiteLeggings>();
		public override string SetbonusText 
			=> "Not getting hit builds up stacks of Virulence\n"
			+ "Virulence charges up every 10 seconds\n"
			+ "Striking while Virulence is charged releases a toxic explosion\n"
			+ "Getting hit depletes Virulence entirely and releases a smaller blast";
		public override SpiritPlayerEffect SetbonusEffect => new BismiteSetbonusEffect();

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 20;
			item.value = Item.buyPrice(silver: 30);
			item.rare = ItemRarityID.Blue;
			item.defense = 3;
		}

		public override void DrawHair(ref bool drawHair, ref bool drawAltHair) => drawAltHair = true;

		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += .04f;
		}

		public override void ArmorSetShadows(Player player)
		{
			if(player.GetSpiritPlayer().setbonus is BismiteSetbonusEffect setbonus && setbonus.virulence <= 0) {
				player.armorEffectDrawShadow = true;
			}
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<BismiteCrystal>(), 6);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}

	public class BismiteSetbonusEffect : SpiritPlayerEffect
	{
		public float virulence = 600f;
		public int projDamage = 15;

		public override void EffectRemoved(Player player)
			=> virulence = 600f;

		public override void PlayerPostUpdate(Player player)
		{
			if(player.HasBuff(ModContent.BuffType<VirulenceCooldown>()) || virulence >= 0) {
				virulence--;
			}

			if(virulence == 0f) {
				Main.PlaySound(new Terraria.Audio.LegacySoundStyle(25, 1));
				Rectangle textPos = new Rectangle((int)player.position.X, (int)player.position.Y - 20, player.width, player.height);
				CombatText.NewText(textPos, new Color(95, 156, 111, 100), "Virulence Charged!");
			}
		}

		public override void PlayerHurt(Player player, bool pvp, bool quiet, double damage, int hitDirection, bool crit)
		{
			virulence = 600f;

			if(!player.HasBuff(ModContent.BuffType<VirulenceCooldown>())) {
				Projectile.NewProjectile(player.position, Vector2.Zero, ModContent.ProjectileType<VirulenceExplosion>(), projDamage, 5, Main.myPlayer);
			}

			player.AddBuff(ModContent.BuffType<VirulenceCooldown>(), 140);
		}

		public override void PlayerOnHitNPC(Player player, Item item, NPC target, int damage, float knockback, bool crit)
		{
			if(virulence <= 0f) {
				Projectile.NewProjectile(target.position, Vector2.Zero, ModContent.ProjectileType<VirulenceExplosion>(), 25, 8, Main.myPlayer);
				virulence = 600f;
				player.AddBuff(ModContent.BuffType<VirulenceCooldown>(), 140);
			}
		}

		public override void PlayerOnHitNPCWithProj(Player player, Projectile proj, NPC target, int damage, float knockback, bool crit)
		{
			if(virulence <= 0f) {
				Projectile.NewProjectile(target.position, Vector2.Zero, ModContent.ProjectileType<VirulenceExplosion>(), 25, 8, Main.myPlayer);
				virulence = 600f;
				player.AddBuff(ModContent.BuffType<VirulenceCooldown>(), 140);
			}
		}

		public override void PlayerDrawEffects(PlayerDrawInfo drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
		{
			var player = drawInfo.drawPlayer;
			if(virulence <= 0 && Main.rand.NextBool(2)) {
				for(int index1 = 0; index1 < 4; ++index1) {
					int dust = Dust.NewDust(player.position, player.width, player.height, 167, 0, 0, 167, default, Main.rand.NextFloat(.4f, 1.2f));
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 1.8f;
					Main.dust[dust].velocity.Y -= 0.5f;
					Main.playerDrawDust.Add(dust);
				}
			}
		}
	}
}
