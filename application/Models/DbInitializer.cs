using System.Linq;
using Microsoft.AspNetCore.Identity;
using STO_Desk_backend.Models.Entities;
using STO_Desk_backend.Models.Enums;

namespace STO_Desk_backend.Models
{
    /// <summary>
    /// Is used for seeding data.
    /// </summary>
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            if (!context.Regions.Any())
            {
                var reg1 = new Region { Name = "Вінницька область" };
                var reg2 = new Region { Name = "Волинська область" };
                var reg3 = new Region { Name = "Дніпропетровська область" };
                var reg4 = new Region { Name = "Донецька область" };
                var reg5 = new Region { Name = "Житомирська область" };
                var reg6 = new Region { Name = "Закарпатська область" };
                var reg7 = new Region { Name = "Запорізька область" };
                var reg8 = new Region { Name = "Івано-Франківська область" };
                var reg9 = new Region { Name = "Київська область" };
                var reg10 = new Region { Name = "Кіровоградська область" };
                var reg11 = new Region { Name = "Луганська область" };
                var reg12 = new Region { Name = "Львівська область" };
                var reg13 = new Region { Name = "Миколаївська область" };
                var reg14 = new Region { Name = "Одеська область" };
                var reg15 = new Region { Name = "Полтавська область" };
                var reg16 = new Region { Name = "Рівненська область" };
                var reg17 = new Region { Name = "Сумська область" };
                var reg18 = new Region { Name = "Тернопільська область" };
                var reg19 = new Region { Name = "Харківська область" };
                var reg20 = new Region { Name = "Херсонська область" };
                var reg21 = new Region { Name = "Хмельницька область" };
                var reg22 = new Region { Name = "Черкаська область" };
                var reg23 = new Region { Name = "Чернівецька область" };
                var reg24 = new Region { Name = "Чернігівська область" };
                var reg25 = new Region { Name = "Автономна Республіка Крим" };
                var reg26 = new Region { Name = "Севастополь" };
                var reg27 = new Region { Name = "Київ" };

                context.Regions.AddRange(
                    reg1, reg2, reg3, reg4, reg5, reg6, reg7, reg8, reg9, reg10,
                    reg11, reg12, reg13, reg14, reg15, reg16, reg17, reg18, reg19, reg20,
                    reg21, reg22, reg23, reg24, reg25, reg26, reg27
                );

                context.SaveChanges();
            }

            if (!context.Users.Any())
            {
                var roleClient = new IdentityRole<int> { Name = "Client", NormalizedName = "CLIENT" };
                var roleMechanic = new IdentityRole<int> { Name = "Mechanic", NormalizedName = "MECHANIC" };
                var roleStoOwner = new IdentityRole<int> { Name = "StoOwner", NormalizedName = "STOOWNER" };
                var roleOperator = new IdentityRole<int> { Name = "Operator", NormalizedName = "OPERATOR" };
                var roleAdmin = new IdentityRole<int> { Name = "Admin", NormalizedName = "ADMIN" };

                context.Roles.AddRange(roleClient, roleMechanic, roleStoOwner, roleOperator, roleAdmin);

                var kyivRegion = context.Regions.FirstOrDefault(r => r.Name == "Київ");

                var hasher = new PasswordHasher<User>();

                var admin = new User
                {
                    UserName = "admin@example.com",
                    NormalizedUserName = "ADMIN@EXAMPLE.COM",
                    Email = "admin@example.com",
                    NormalizedEmail = "ADMIN@EXAMPLE.COM",
                    EmailConfirmed = true,
                    Region = kyivRegion
                };
                admin.PasswordHash = hasher.HashPassword(admin, "Admin123!");

                var op = new User
                {
                    UserName = "operator@example.com",
                    NormalizedUserName = "OPERATOR@EXAMPLE.COM",
                    Email = "operator@example.com",
                    NormalizedEmail = "OPERATOR@EXAMPLE.COM",
                    EmailConfirmed = true,
                    Region = kyivRegion
                };
                op.PasswordHash = hasher.HashPassword(op, "Operator123!");

                var owner = new User
                {
                    UserName = "owner@example.com",
                    NormalizedUserName = "OWNER@EXAMPLE.COM",
                    Email = "owner@example.com",
                    NormalizedEmail = "OWNER@EXAMPLE.COM",
                    EmailConfirmed = true,
                    Region = kyivRegion
                };
                owner.PasswordHash = hasher.HashPassword(owner, "Owner123!");

                context.Users.AddRange(admin, op, owner);

                var testSto = new Sto
                {
                    Name = "Тестове СТО",
                    Body = "Опис тестового СТО",
                    Owner = owner,
                    Region = kyivRegion
                };

                context.Stos.Add(testSto);
                context.SaveChanges();

                context.UserRoles.AddRange(
                    new IdentityUserRole<int> { UserId = admin.Id, RoleId = roleAdmin.Id },
                    new IdentityUserRole<int> { UserId = op.Id, RoleId = roleOperator.Id },
                    new IdentityUserRole<int> { UserId = owner.Id, RoleId = roleStoOwner.Id }
                );

                context.SaveChanges();
            }

            if (!context.RoleTicketCategories.Any())
            {
                var roleStoCat = new RoleTicketCategory { Name = "Ролі СТО" };
                var roleAppCat = new RoleTicketCategory { Name = "Ролі Додатку" };

                context.RoleTicketCategories.AddRange(roleStoCat, roleAppCat);

                context.RoleTicketThemes.AddRange(
                    new RoleTicketTheme { Name = "Роль механіка", TargetRole = TargetRole.Mechanic, Category = roleStoCat },
                    new RoleTicketTheme { Name = "Роль власника СТО", TargetRole = TargetRole.StoOwner, Category = roleStoCat },
                    new RoleTicketTheme { Name = "Новий власник СТО", TargetRole = TargetRole.NewStoOwner, Category = roleStoCat },
                    new RoleTicketTheme { Name = "Роль оператора додатку", TargetRole = TargetRole.Operator, Category = roleAppCat },
                    new RoleTicketTheme { Name = "Роль адміністратора додатку", TargetRole = TargetRole.Admin, Category = roleAppCat }
                );

                context.SaveChanges();
            }

            if (!context.TicketCategories.Any())
            {
                var cat1 = new TicketCategory { Name = "Освітлення" };
                var cat2 = new TicketCategory { Name = "Двигун" };
                var cat3 = new TicketCategory { Name = "Гальмівна система" };
                var cat4 = new TicketCategory { Name = "Колеса та підвіска" };
                var cat5 = new TicketCategory { Name = "Електрика" };
                var cat6 = new TicketCategory { Name = "Трансмісія" };
                var cat7 = new TicketCategory { Name = "Кермо" };
                var cat8 = new TicketCategory { Name = "Охолодження та рідини" };
                var cat9 = new TicketCategory { Name = "Кондиціонер / клімат" };
                var cat10 = new TicketCategory { Name = "Вихлопна система" };
                var cat11 = new TicketCategory { Name = "Інше" };

                context.TicketCategories.AddRange(cat1, cat2, cat3, cat4, cat5, cat6, cat7, cat8, cat9, cat10, cat11);

                context.TicketThemes.AddRange(
                    new TicketTheme { Name = "Проблема з передніми фарами", Category = cat1 },
                    new TicketTheme { Name = "Проблема із задніми фарами", Category = cat1 },
                    new TicketTheme { Name = "Проблема з поворотниками", Category = cat1 },
                    new TicketTheme { Name = "Проблема зі стоп-сигналами", Category = cat1 },

                    new TicketTheme { Name = "Проблема з двигуном", Category = cat2 },
                    new TicketTheme { Name = "Двигун не заводиться", Category = cat2 },
                    new TicketTheme { Name = "Сторонні шуми в двигуні", Category = cat2 },
                    new TicketTheme { Name = "Перегрів двигуна", Category = cat2 },

                    new TicketTheme { Name = "Проблема з гальмами", Category = cat3 },
                    new TicketTheme { Name = "Скрип гальм", Category = cat3 },
                    new TicketTheme { Name = "Погане гальмування", Category = cat3 },
                    new TicketTheme { Name = "Провал педалі гальма", Category = cat3 },

                    new TicketTheme { Name = "Проблема з колесами", Category = cat4 },
                    new TicketTheme { Name = "Прокол шини", Category = cat4 },
                    new TicketTheme { Name = "Нерівномірний знос шин", Category = cat4 },
                    new TicketTheme { Name = "Проблема з підвіскою", Category = cat4 },
                    new TicketTheme { Name = "Стукіт у підвісці", Category = cat4 },

                    new TicketTheme { Name = "Проблема з акумулятором", Category = cat5 },
                    new TicketTheme { Name = "Акумулятор розряджається", Category = cat5 },
                    new TicketTheme { Name = "Проблема з генератором", Category = cat5 },
                    new TicketTheme { Name = "Проблема з електрикою", Category = cat5 },

                    new TicketTheme { Name = "Проблема з коробкою передач", Category = cat6 },
                    new TicketTheme { Name = "Проблема з перемиканням передач", Category = cat6 },
                    new TicketTheme { Name = "Проблема зі зчепленням", Category = cat6 },

                    new TicketTheme { Name = "Проблема з кермом", Category = cat7 },
                    new TicketTheme { Name = "Важке керування", Category = cat7 },
                    new TicketTheme { Name = "Вібрація керма", Category = cat7 },

                    new TicketTheme { Name = "Витік рідини", Category = cat8 },
                    new TicketTheme { Name = "Проблема з охолодженням", Category = cat8 },
                    new TicketTheme { Name = "Низький рівень масла", Category = cat8 },

                    new TicketTheme { Name = "Проблема з кондиціонером", Category = cat9 },
                    new TicketTheme { Name = "Не працює обігрів", Category = cat9 },

                    new TicketTheme { Name = "Проблема з вихлопною системою", Category = cat10 },
                    new TicketTheme { Name = "Сильний дим з вихлопу", Category = cat10 },

                    new TicketTheme { Name = "Сторонній шум", Category = cat11 },
                    new TicketTheme { Name = "Інша проблема", Category = cat11 }
                );

                context.SaveChanges();
            }
        }
    }
}
