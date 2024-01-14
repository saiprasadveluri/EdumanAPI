﻿// <auto-generated />
using System;
using EduManAPI;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EduManAPI.Migrations
{
    [DbContext(typeof(EduManDBContext))]
    [Migration("20201119031653_DigitalContent_tbl_Creation")]
    partial class DigitalContent_tbl_Creation
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EduManAPI.AcdYear", b =>
                {
                    b.Property<Guid>("AcdId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AcdText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AcdId");

                    b.ToTable("AcdYears");
                });

            modelBuilder.Entity("EduManAPI.Chalan", b =>
                {
                    b.Property<Guid>("ChlId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ChlDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("MapId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ChlId");

                    b.HasIndex("MapId");

                    b.ToTable("Chalans");
                });

            modelBuilder.Entity("EduManAPI.ChalanLineInfo", b =>
                {
                    b.Property<Guid>("ChlLineId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ChlId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("DueMon")
                        .HasColumnType("int");

                    b.Property<string>("FeeHeadName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("FeeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("PaidAmt")
                        .HasColumnType("float");

                    b.Property<int>("TermNo")
                        .HasColumnType("int");

                    b.Property<double>("TotAmt")
                        .HasColumnType("float");

                    b.HasKey("ChlLineId");

                    b.HasIndex("ChlId");

                    b.HasIndex("FeeId");

                    b.ToTable("ChalanLineInfos");
                });

            modelBuilder.Entity("EduManAPI.DigitalContent", b =>
                {
                    b.Property<Guid>("DCId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ChapId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ContentPath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DCType")
                        .HasColumnType("int");

                    b.Property<string>("MimeType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DCId");

                    b.HasIndex("ChapId");

                    b.ToTable("DigitalContents");
                });

            modelBuilder.Entity("EduManAPI.Enquiry", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ContactName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EnqDate")
                        .HasColumnType("date");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SchoolName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Enquiries");
                });

            modelBuilder.Entity("EduManAPI.FeeCollection", b =>
                {
                    b.Property<Guid>("FeeColId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ColDate")
                        .HasColumnType("date");

                    b.Property<Guid>("MapId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PayType")
                        .HasColumnType("int");

                    b.HasKey("FeeColId");

                    b.HasIndex("MapId");

                    b.ToTable("FeeCollections");
                });

            modelBuilder.Entity("EduManAPI.FeeCollectionLineItem", b =>
                {
                    b.Property<Guid>("LineItemId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<Guid>("ColId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FeeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LineItemId");

                    b.HasIndex("ColId");

                    b.HasIndex("FeeId");

                    b.ToTable("FeeCollectionLineItems");
                });

            modelBuilder.Entity("EduManAPI.FeeConcession", b =>
                {
                    b.Property<Guid>("ConId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<Guid>("FeeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("MapId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ConId");

                    b.HasIndex("MapId");

                    b.HasIndex("FeeId", "MapId")
                        .IsUnique();

                    b.ToTable("FeeConcessions");
                });

            modelBuilder.Entity("EduManAPI.FeeHeadMaster", b =>
                {
                    b.Property<Guid>("FeeHeadId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FeeHeadName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FeeType")
                        .HasColumnType("int");

                    b.Property<Guid>("OrgId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Terms")
                        .HasColumnType("int");

                    b.HasKey("FeeHeadId");

                    b.HasIndex("OrgId");

                    b.ToTable("FeeHeadMasters");
                });

            modelBuilder.Entity("EduManAPI.FeeMaster", b =>
                {
                    b.Property<Guid>("FeeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AcdyearId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<int>("DueDayNo")
                        .HasColumnType("int");

                    b.Property<int>("DueMonthNo")
                        .HasColumnType("int");

                    b.Property<Guid>("FHeadId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("MapId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("StnId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("TermNo")
                        .HasColumnType("int");

                    b.HasKey("FeeId");

                    b.HasIndex("AcdyearId");

                    b.HasIndex("MapId");

                    b.HasIndex("StnId");

                    b.ToTable("FeeMasters");
                });

            modelBuilder.Entity("EduManAPI.OnLineSessionInfo", b =>
                {
                    b.Property<Guid>("OSId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("date");

                    b.Property<TimeSpan>("EndTime")
                        .HasColumnType("time");

                    b.Property<int>("IsReapated")
                        .HasColumnType("int");

                    b.Property<Guid>("OSessionUrlId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("RepeatString")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("date");

                    b.Property<TimeSpan>("StartTime")
                        .HasColumnType("time");

                    b.Property<Guid>("SubMapId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("OSId");

                    b.HasIndex("OSessionUrlId");

                    b.HasIndex("SubMapId");

                    b.ToTable("OnLineSessionInfos");
                });

            modelBuilder.Entity("EduManAPI.OnlineSessionAttendee", b =>
                {
                    b.Property<Guid>("AtId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("EvtDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("MapId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("OSId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("AtId");

                    b.HasIndex("MapId");

                    b.HasIndex("OSId");

                    b.ToTable("OnlineSessionAttendees");
                });

            modelBuilder.Entity("EduManAPI.OnlineSessionUrl", b =>
                {
                    b.Property<Guid>("OSUrlId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SessionUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("TeacherId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("OSUrlId");

                    b.HasIndex("TeacherId");

                    b.ToTable("OnlineSessionUrls");
                });

            modelBuilder.Entity("EduManAPI.Organization", b =>
                {
                    b.Property<Guid>("OrgId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("OrgAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrgEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrgMobile")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrgName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrgPOC")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("OrgId");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("EduManAPI.RoleMaster", b =>
                {
                    b.Property<Guid>("RoleID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleVal")
                        .HasColumnType("int");

                    b.HasKey("RoleID");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("EduManAPI.Standard", b =>
                {
                    b.Property<Guid>("StdId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("OrgId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("StdName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("StdId");

                    b.HasIndex("OrgId", "StdName")
                        .IsUnique();

                    b.ToTable("Standards");
                });

            modelBuilder.Entity("EduManAPI.StdSubMap", b =>
                {
                    b.Property<Guid>("MapId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("StdId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("SubId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("MapId");

                    b.HasIndex("SubId");

                    b.HasIndex("StdId", "SubId")
                        .IsUnique()
                        .HasFilter("[StdId] IS NOT NULL AND [SubId] IS NOT NULL");

                    b.ToTable("StdSubMaps");
                });

            modelBuilder.Entity("EduManAPI.StuStdAcdYearMap", b =>
                {
                    b.Property<Guid>("MapId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AcYearId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("RecDate")
                        .HasColumnType("date");

                    b.Property<int>("RecType")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<Guid>("StnId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("StuId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("MapId");

                    b.HasIndex("StnId");

                    b.HasIndex("StuId");

                    b.HasIndex("AcYearId", "StnId", "StuId")
                        .IsUnique();

                    b.ToTable("StuStdAcdYearMaps");
                });

            modelBuilder.Entity("EduManAPI.StudentInfo", b =>
                {
                    b.Property<Guid>("StuId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AadharNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BloodGroup")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Cast")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DOAdmission")
                        .HasColumnType("date");

                    b.Property<DateTime>("DOBirth")
                        .HasColumnType("date");

                    b.Property<string>("FName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FatherMobile")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FatherName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.Property<string>("LName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("LoginUID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("MName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MotherName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ParentEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RegdNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Religion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ResAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SchoolAdmNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("StuFatherImageFile")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("StuImageFile")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("StuMotherImageFile")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("StuId");

                    b.HasIndex("LoginUID");

                    b.ToTable("StudentInfos");
                });

            modelBuilder.Entity("EduManAPI.SubChapeter", b =>
                {
                    b.Property<Guid>("ChapId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ChapName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("MapId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ChapId");

                    b.HasIndex("MapId", "ChapName")
                        .IsUnique();

                    b.ToTable("SubChapeters");
                });

            modelBuilder.Entity("EduManAPI.Subject", b =>
                {
                    b.Property<Guid>("SubId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("OrgId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SubjectName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("SubId");

                    b.HasIndex("OrgId", "SubjectName")
                        .IsUnique();

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("EduManAPI.Teacher", b =>
                {
                    b.Property<Guid>("TeacherId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BllodGroup")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DOJoining")
                        .HasColumnType("date");

                    b.Property<string>("EmpId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FatherName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("LoginUID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("MName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MobileNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MotherName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("OrgId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.Property<string>("TeacherType")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TeacherId");

                    b.HasIndex("LoginUID");

                    b.HasIndex("OrgId", "EmpId")
                        .IsUnique();

                    b.ToTable("Teachers");
                });

            modelBuilder.Entity("EduManAPI.UserInfo", b =>
                {
                    b.Property<Guid>("ID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Emailid")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MobNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("UserInfos");
                });

            modelBuilder.Entity("EduManAPI.UserOrgMap", b =>
                {
                    b.Property<Guid>("MapId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("OrgId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("MapId");

                    b.HasIndex("OrgId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId", "OrgId", "RoleId")
                        .IsUnique();

                    b.ToTable("UserOrgMaps");
                });

            modelBuilder.Entity("EduManAPI.Chalan", b =>
                {
                    b.HasOne("EduManAPI.StuStdAcdYearMap", "SSMap")
                        .WithMany()
                        .HasForeignKey("MapId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EduManAPI.ChalanLineInfo", b =>
                {
                    b.HasOne("EduManAPI.Chalan", "Chln")
                        .WithMany()
                        .HasForeignKey("ChlId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EduManAPI.FeeMaster", "Fee")
                        .WithMany()
                        .HasForeignKey("FeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EduManAPI.DigitalContent", b =>
                {
                    b.HasOne("EduManAPI.SubChapeter", "Chap")
                        .WithMany()
                        .HasForeignKey("ChapId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EduManAPI.FeeCollection", b =>
                {
                    b.HasOne("EduManAPI.StuStdAcdYearMap", "SSMap")
                        .WithMany()
                        .HasForeignKey("MapId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EduManAPI.FeeCollectionLineItem", b =>
                {
                    b.HasOne("EduManAPI.FeeCollection", "FC")
                        .WithMany()
                        .HasForeignKey("ColId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EduManAPI.FeeMaster", "FM")
                        .WithMany()
                        .HasForeignKey("FeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EduManAPI.FeeConcession", b =>
                {
                    b.HasOne("EduManAPI.StuStdAcdYearMap", "SSMap")
                        .WithMany()
                        .HasForeignKey("MapId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EduManAPI.FeeHeadMaster", b =>
                {
                    b.HasOne("EduManAPI.Organization", "Org")
                        .WithMany()
                        .HasForeignKey("OrgId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EduManAPI.FeeMaster", b =>
                {
                    b.HasOne("EduManAPI.AcdYear", "AcdYear")
                        .WithMany()
                        .HasForeignKey("AcdyearId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EduManAPI.StuStdAcdYearMap", "SSMap")
                        .WithMany()
                        .HasForeignKey("MapId");

                    b.HasOne("EduManAPI.Standard", "Stnd")
                        .WithMany()
                        .HasForeignKey("StnId");
                });

            modelBuilder.Entity("EduManAPI.OnLineSessionInfo", b =>
                {
                    b.HasOne("EduManAPI.OnlineSessionUrl", "SUrl")
                        .WithMany()
                        .HasForeignKey("OSessionUrlId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EduManAPI.StdSubMap", "SSMap")
                        .WithMany()
                        .HasForeignKey("SubMapId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EduManAPI.OnlineSessionAttendee", b =>
                {
                    b.HasOne("EduManAPI.StuStdAcdYearMap", "StuMap")
                        .WithMany()
                        .HasForeignKey("MapId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EduManAPI.OnLineSessionInfo", "OSInfo")
                        .WithMany()
                        .HasForeignKey("OSId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EduManAPI.OnlineSessionUrl", b =>
                {
                    b.HasOne("EduManAPI.Teacher", "Tchr")
                        .WithMany()
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EduManAPI.Standard", b =>
                {
                    b.HasOne("EduManAPI.Organization", "Org")
                        .WithMany()
                        .HasForeignKey("OrgId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EduManAPI.StdSubMap", b =>
                {
                    b.HasOne("EduManAPI.Standard", "Std")
                        .WithMany()
                        .HasForeignKey("StdId");

                    b.HasOne("EduManAPI.Subject", "Sub")
                        .WithMany()
                        .HasForeignKey("SubId");
                });

            modelBuilder.Entity("EduManAPI.StuStdAcdYearMap", b =>
                {
                    b.HasOne("EduManAPI.AcdYear", "AcYear")
                        .WithMany()
                        .HasForeignKey("AcYearId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EduManAPI.Standard", "Stn")
                        .WithMany()
                        .HasForeignKey("StnId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EduManAPI.StudentInfo", "Stu")
                        .WithMany()
                        .HasForeignKey("StuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EduManAPI.StudentInfo", b =>
                {
                    b.HasOne("EduManAPI.UserInfo", "UInfo")
                        .WithMany()
                        .HasForeignKey("LoginUID");
                });

            modelBuilder.Entity("EduManAPI.SubChapeter", b =>
                {
                    b.HasOne("EduManAPI.StdSubMap", "StdSubMp")
                        .WithMany()
                        .HasForeignKey("MapId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EduManAPI.Subject", b =>
                {
                    b.HasOne("EduManAPI.Organization", "Org")
                        .WithMany()
                        .HasForeignKey("OrgId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EduManAPI.Teacher", b =>
                {
                    b.HasOne("EduManAPI.UserInfo", "UInfo")
                        .WithMany()
                        .HasForeignKey("LoginUID");

                    b.HasOne("EduManAPI.Organization", "Org")
                        .WithMany()
                        .HasForeignKey("OrgId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EduManAPI.UserOrgMap", b =>
                {
                    b.HasOne("EduManAPI.Organization", "OrgInfo")
                        .WithMany()
                        .HasForeignKey("OrgId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EduManAPI.RoleMaster", "RoleInfo")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EduManAPI.UserInfo", "Uinfo")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
