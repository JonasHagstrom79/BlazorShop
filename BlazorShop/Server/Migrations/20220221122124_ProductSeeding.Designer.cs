﻿// <auto-generated />
using BlazorShop.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BlazorShop.Server.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20220221122124_ProductSeeding")]
    partial class ProductSeeding
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("BlazorShop.Shared.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "The Hitchhiker's Guide to the Galaxy[note 1] (sometimes referred to as HG2G,[1] HHGTTG,[2] H2G2,[3] or tHGttG) is a comedy science fiction franchise created by Douglas Adams. Originally a 1978 radio comedy broadcast on BBC Radio 4, it was later adapted to other formats, including stage shows, novels, comic books, a 1981 TV series, a 1984 text-based computer game, and 2005 feature film. ",
                            ImageUrl = "https://upload.wikimedia.org/wikipedia/en/b/bd/H2G2_UK_front_cover.jpg",
                            Price = 9.99m,
                            Title = "The Hitchhiker's Guide to the Galaxy"
                        },
                        new
                        {
                            Id = 2,
                            Description = "Ivanhoe: A Romance (/ˈaɪvənˌhoʊ/) by Walter Scott is a historical novel published in three volumes, in 1819, as one of the Waverley novels. At the time it was written, the novel represented a shift by Scott away from writing novels set in Scotland in the fairly recent past to England in the Middle Ages. Ivanhoe proved to be one of the best-known and most influential of Scott's novels. ",
                            ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/22/Ivanhoe_title_page.jpg/220px-Ivanhoe_title_page.jpg",
                            Price = 5.99m,
                            Title = "Ivanhoe"
                        },
                        new
                        {
                            Id = 3,
                            Description = "The Silmarillion (Quenya: [silmaˈrilliɔn]) is a collection of mythopoeic stories by the English writer J. R. R. Tolkien, edited and published posthumously by his son Christopher Tolkien in 1977 with assistance from the fantasy author Guy Gavriel Kay.[T 1] The Silmarillion tells of Eä, a fictional universe that includes the Blessed Realm of Valinor, the once-great region of Beleriand, the sunken island of Númenor, and the continent of Middle-earth, where Tolkien's most popular works—The Hobbit and The Lord of the Rings—take place. ",
                            ImageUrl = "https://upload.wikimedia.org/wikipedia/en/thumb/d/db/Silmarillion.png/220px-Silmarillion.png",
                            Price = 8.99m,
                            Title = "The Silmarillion"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
