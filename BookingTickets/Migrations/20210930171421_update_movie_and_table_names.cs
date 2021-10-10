using Microsoft.EntityFrameworkCore.Migrations;

namespace BookingTickets.Migrations
{
    public partial class update_movie_and_table_names : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Screening_ScreeningId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Seat_SeatId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Room_Cinemas_CinemaId",
                table: "Room");

            migrationBuilder.DropForeignKey(
                name: "FK_Screening_Movies_MovieId",
                table: "Screening");

            migrationBuilder.DropForeignKey(
                name: "FK_Screening_Room_RoomId",
                table: "Screening");

            migrationBuilder.DropForeignKey(
                name: "FK_Seat_Room_RoomId",
                table: "Seat");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Seat",
                table: "Seat");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Screening",
                table: "Screening");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Room",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Movies");

            migrationBuilder.RenameTable(
                name: "Seat",
                newName: "Seats");

            migrationBuilder.RenameTable(
                name: "Screening",
                newName: "Screenings");

            migrationBuilder.RenameTable(
                name: "Room",
                newName: "Rooms");

            migrationBuilder.RenameIndex(
                name: "IX_Seat_RoomId",
                table: "Seats",
                newName: "IX_Seats_RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Screening_RoomId",
                table: "Screenings",
                newName: "IX_Screenings_RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Screening_MovieId",
                table: "Screenings",
                newName: "IX_Screenings_MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_Room_CinemaId",
                table: "Rooms",
                newName: "IX_Rooms_CinemaId");

            migrationBuilder.AlterColumn<string>(
                name: "Cover",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Seats",
                table: "Seats",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Screenings",
                table: "Screenings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rooms",
                table: "Rooms",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Screenings_ScreeningId",
                table: "Reservations",
                column: "ScreeningId",
                principalTable: "Screenings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Seats_SeatId",
                table: "Reservations",
                column: "SeatId",
                principalTable: "Seats",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Cinemas_CinemaId",
                table: "Rooms",
                column: "CinemaId",
                principalTable: "Cinemas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Screenings_Movies_MovieId",
                table: "Screenings",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Screenings_Rooms_RoomId",
                table: "Screenings",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_Rooms_RoomId",
                table: "Seats",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Screenings_ScreeningId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Seats_SeatId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Cinemas_CinemaId",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Screenings_Movies_MovieId",
                table: "Screenings");

            migrationBuilder.DropForeignKey(
                name: "FK_Screenings_Rooms_RoomId",
                table: "Screenings");

            migrationBuilder.DropForeignKey(
                name: "FK_Seats_Rooms_RoomId",
                table: "Seats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Seats",
                table: "Seats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Screenings",
                table: "Screenings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rooms",
                table: "Rooms");

            migrationBuilder.RenameTable(
                name: "Seats",
                newName: "Seat");

            migrationBuilder.RenameTable(
                name: "Screenings",
                newName: "Screening");

            migrationBuilder.RenameTable(
                name: "Rooms",
                newName: "Room");

            migrationBuilder.RenameIndex(
                name: "IX_Seats_RoomId",
                table: "Seat",
                newName: "IX_Seat_RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Screenings_RoomId",
                table: "Screening",
                newName: "IX_Screening_RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Screenings_MovieId",
                table: "Screening",
                newName: "IX_Screening_MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_Rooms_CinemaId",
                table: "Room",
                newName: "IX_Room_CinemaId");

            migrationBuilder.AlterColumn<string>(
                name: "Cover",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Movies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Seat",
                table: "Seat",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Screening",
                table: "Screening",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Room",
                table: "Room",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Screening_ScreeningId",
                table: "Reservations",
                column: "ScreeningId",
                principalTable: "Screening",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Seat_SeatId",
                table: "Reservations",
                column: "SeatId",
                principalTable: "Seat",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Room_Cinemas_CinemaId",
                table: "Room",
                column: "CinemaId",
                principalTable: "Cinemas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Screening_Movies_MovieId",
                table: "Screening",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Screening_Room_RoomId",
                table: "Screening",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Seat_Room_RoomId",
                table: "Seat",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
