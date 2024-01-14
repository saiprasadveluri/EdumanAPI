using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Security.AccessControl;

namespace EduManAPI
{
    public class EduManDBContext : DbContext
    {
        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<RoleMaster> Roles { get; set; }
        public DbSet<UserOrgMap> UserOrgMaps { get; set; }
        public DbSet<Enquiry> Enquiries { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Standard> Standards { get; set; }

        public DbSet<StdSubMap> StdSubMaps { get; set; }
        public DbSet<SubChapeter> SubChapeters { get; set; }
        public DbSet<AcdYear> AcdYears { get; set; }
        public DbSet<StudentInfo> StudentInfos { get; set; }
        public DbSet<StuStdAcdYearMap> StuStdAcdYearMaps { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<FeeHeadMaster> FeeHeadMasters { get; set; }
        public DbSet<FeeMaster> FeeMasters { get; set; }
        public DbSet<FeeCollection> FeeCollections { get; set; }
        public DbSet<FeeCollectionLineItem> FeeCollectionLineItems { get; set; }
        public DbSet<FeeConcession> FeeConcessions { get; set; }
        public DbSet<Chalan> Chalans { get; set; }
        public DbSet<ChalanLineInfo> ChalanLineInfos { get; set; }
        public DbSet<OnlineSessionUrl> OnlineSessionUrls { get; set; }
        public DbSet<OnLineSessionInfo> OnLineSessionInfos { get; set; }
        public DbSet<OnlineSessionAttendee> OnlineSessionAttendees { get; set; }
        public DbSet<HomeWork> HomeWorks { get; set; }
        public DbSet<HomeWorkSubmission> HomeWorkSubmissions { get; set; }
        public DbSet<Assignment> Assignments { get; set; }

        public DbSet<AssignmentSubmission> AssignmentSubmissions { get; set; }

        public DbSet<DigitalContent> DigitalContents { get; set; }

        public DbSet<ExamType> ExamTypes { get; set; }

        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamSchedule> ExamSchedules { get; set; }
        public DbSet<ChapterwiseExam> ChatpterwiseExams { get; set; }
        public DbSet<ChapterwiseExamChapter> ChapterwiseExamChapters { get; set; }
        
        public DbSet<ExamProgressReport> ExamProgressReports { get; set; }
        public DbSet<ExamProgressReportHead> ExamProgressReportHeads { get; set; }
        public DbSet<GradeRange> GradeRanges { get; set; }
        public DbSet<CoCurricularMark> CoCurricularMarks { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationRecp> NotificationRecps { get; set; }
        public DbSet<AppLinkDistribution> AppLinkDistributions { get; set; }
        public DbSet<TimeTable> TimeTables { get; set; }
        public DbSet<TimeTableSetting> TimeTableSettings { get; set; }
        public DbSet<SchoolSetting> SchoolSettings { get; set; }
        public DbSet<Cashier> Cashiers { get; set; }
        public DbSet<Payhead> Payheads { get; set; }
        public DbSet<PayRoll> PayRolls { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<SalaryDetail> SalaryDetails { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<ExpenseHead> ExpenseHeads { get; set; }

        public DbSet<BankDeatail> BankDeatails { get; set; }
        public DbSet<Tax> Taxes { get; set; }
        public DbSet<ServiceOrProduct> ServiceOrProducts { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillLineItem> BillLineItems { get; set; }

        public DbSet<LanguageMaster> LanguageMasters { get; set; }
        public DbSet<StudentLangMap> studentLangMaps { get; set; }

        public DbSet<PettyExpnse> PettyExpnses { get; set; }
        public EduManDBContext(DbContextOptions opts) : base(opts)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Organization>().HasIndex(p => new { p.OrgCode}).IsUnique(true);
            modelBuilder.Entity<UserOrgMap>().HasIndex(p => new { p.UserId, p.OrgId, p.RoleId }).IsUnique(true);
            modelBuilder.Entity<Standard>().HasIndex(p => new { p.OrgId, p.StdName }).IsUnique(true);
            modelBuilder.Entity<Subject>().HasIndex(p => new { p.OrgId, p.SubjectName }).IsUnique(true);
            modelBuilder.Entity<StdSubMap>().HasIndex(p => new { p.StdId, p.SubId }).IsUnique(true);
            modelBuilder.Entity<SubChapeter>().HasIndex(p => new { p.MapId, p.ChapName }).IsUnique(true);
            //modelBuilder.Entity<AcdYear>().HasIndex(p => new { p.OrgId, p.AcdText }).IsUnique(true);
            modelBuilder.Entity<StudentInfo>().Property(p => p.IsActive).HasDefaultValue(1);
            modelBuilder.Entity<StuStdAcdYearMap>().HasIndex(p => new { p.AcYearId, p.StnId, p.StuId }).IsUnique(true);
            modelBuilder.Entity<StuStdAcdYearMap>().Property(p => p.RecType).HasDefaultValue(0);
            modelBuilder.Entity<Teacher>().Property(p => p.Status).HasDefaultValue(1);//1: Active
            modelBuilder.Entity<Teacher>().HasIndex(p => new { p.OrgId, p.EmpId }).IsUnique(true);
            //modelBuilder.Entity<FeeMaster>().HasIndex(p => new { p.FHeadId,p.TermNo}).IsUnique(true);
            //modelBuilder.Entity<FeeCollection>().HasIndex(p => new { p.FeeId, p.MapId }).IsUnique(true);
            modelBuilder.Entity<FeeConcession>().HasIndex(p => new { p.FeeId, p.MapId }).IsUnique(true);
            modelBuilder.Entity<Chalan>().HasIndex(c => c.ChlnNumber).IsUnique(true);
            modelBuilder.Entity<Chalan>().Property(p=>p.ChalanStatus).HasDefaultValue(1);
            modelBuilder.Entity<FeeCollection>().HasIndex(m => m.ChlnId).IsUnique(true);
            modelBuilder.Entity<TimeTableSetting>().HasIndex(m => m.StnId).IsUnique(true);
            modelBuilder.Entity<Cashier>().HasIndex(p => new { p.OrgId, p.EmpId }).IsUnique(true);
            modelBuilder.Entity<Payhead>().HasIndex(p => new { p.OrgId, p.Name }).IsUnique(true);
            modelBuilder.Entity<PayRoll>().HasIndex(p => new { p.HID, p.EmpID }).IsUnique(true);
            modelBuilder.Entity<Salary>().HasIndex(p => new {p.EmpId,p.MonthNo,p.Year}).IsUnique(true);
            modelBuilder.Entity<StudentLangMap>().HasIndex(s => new { s.StudentStnAcdMapId, s.LangOrdinal }).IsUnique(true);
            modelBuilder.Entity<StudentLangMap>().HasIndex(s => new { s.StudentStnAcdMapId, s.LanguageId }).IsUnique(true);

            modelBuilder.Entity<Subject>().HasIndex(su => new { su.OrgId, su.SubjectName }).IsUnique(true);            
        }
    }

    public class PettyExpnse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid PEID { get; set; }
        [Required]
        public Guid OrgId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public Guid EHeadID{ get; set; }
        [Required]
        public string PaymentMode { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public Guid AccountTypeID { get; set; }        
        public string ChequeOrRefNo { get; set; }
        [Required]
        public DateTime PaymentDate { get; set; }
        public string PaidTo { get; set; }
        public string Ms { get; set; }
        public string Comments { get; set; }
        public string FileGuid { get; set; }
        public string ContentType { get; set; }

        [ForeignKey("EHeadID")]
        public ExpenseHead ExpHead { get; set; }
        [ForeignKey("AccountTypeID")]
        public BankDeatail BankDet { get; set; }
        [ForeignKey("OrgId")]
        public Organization Org { get; set; }
    }

    public class Bill
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid BID { get; set; }
        [Required]
        public Guid OrgId { get; set; }
        [Required]
        public Guid VendorId { get; set; }
        [Required]
        public Guid ExpHeadId { get; set; }
        [Required]
        public DateTime BillDate { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        [Required]
        public string BillNumber { get; set; }
        public string FileGuid { get; set; }
        public string FileMIME { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }
        public int DiscountType { get; set; }//1: Amount, 2: Percentage
        public double DiscountVal { get; set; }

        [ForeignKey("ExpHeadId")]
        public ExpenseHead Exp { get; set; }
        
        [ForeignKey("OrgId")]
        public Organization Org { get; set; }
        [ForeignKey("VendorId")]
        public Vendor Ven { get; set; }
    }

    public class BillLineItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid BLID { get; set; }
        [Required]
        public Guid BID { get; set; }
        [Required]
        public Guid ProdId { get; set; }
        [Required]
        public Guid TaxId { get; set; }
        [Required]
        public int Quantity { get; set; }
        public string Description { get; set; }
        [ForeignKey("BID")]
        public Bill bill { get; set; }
        [ForeignKey("ProdId")]
        public ServiceOrProduct Prod { get; set; }
        [ForeignKey("TaxId")]
        public Tax tax { get; set; }

    }
    public class ServiceOrProduct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid PID { get; set; }
        [Required]
        public Guid OrgId { get; set; }
        [Required]
        public Guid EHId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        public string Description { get; set; }

        [Required]
        public Guid TID { get; set; }
        [ForeignKey("EHId")]
        public ExpenseHead Exp { get; set; }
        [ForeignKey("TID")]
        public Tax TaxObj { get; set; }

        [ForeignKey("OrgId")]
        public Organization Org { get; set; }
    }
    public class Tax
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid TID { get; set; }
        [Required]
        public Guid OrgId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public double Percentage { get; set; }
        [Required]
        public string TaxNumber { get; set; }
        public string Description { get; set; }
        [ForeignKey("OrgId")]
        public Organization Org { get; set; }

    }
    public class BankDeatail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid BID { get; set; }
        [Required]
        public Guid OrgId { get; set; }
        [Required]
        public int AccountType { get; set; }
        public string CashierName { get; set; }

        public string BankName { get; set; }
        public string BankAccountNo { get; set; }
        public string WalletName { get; set; }
        public string MobileNo { get; set; }
        [Required]
        public int Status { get; set; }
        [ForeignKey("OrgId")]
        public Organization Org { get; set; }

    }

    public class ExpenseHead
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid EHID { get; set; }
        [Required]
        public Guid OrgId { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public int Status { get; set; }

        public string Note { get; set; }

        [ForeignKey("OrgId")]
        public Organization Org { get; set; }

    }
    public class Payhead
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid PHID { get; set; }
        [Required]
        public Guid OrgId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Type { get; set; }

        [ForeignKey("OrgId")]
        public Organization Org { get; set; }

    }

    public class Vendor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid VendorId { get; set; }
        [Required]
        public Guid OrgId { get; set; }
        [Required]
        public string VendorName { get; set; }
        [Required]
        public string ContactName { get; set; }
        [Required]
        public string ContactNo { get; set; }
        [Required]
        public string Location { get; set; }
        public string Address { get; set; }
        [Required]
        public int Status { get; set; }
        [ForeignKey("OrgId")]
        public Organization Org { get; set; }
    }

    public class PayRoll
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid PRID { get; set; }
        [Required]
        public Guid HID { get; set; }
        [Required]
        public Guid EmpID { get; set; }
        [Required]
        public double OrgAmount { get; set; }
        [Required]
        public int Status { get; set; }
        [ForeignKey("HID")]
        public Payhead PHead { get; set; }

    }

    public class Salary
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid SALID { get; set; }

        [Required]
        public Guid EmpId { get; set; }
        [Required]
        public int MonthNo { get; set; }
        [Required]
        public string Year { get; set; }
        [Required]
        public int Status { get; set; }//0: Pending 1: Approved
    }

    public class SalaryDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid SALDetID { get; set; }
        [Required]
        public Guid SalID { get; set; }
        [Required]
        public string HeadName { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public int HeadType { get; set; }//0: Deduction, 1: Earning
        [ForeignKey("SalID")]
        public Salary Sal { get; set; }
    }


    public class SchoolSetting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid SSID { get; set; }
        [Required]
        public Guid OrgId { get; set; }
        public string PrintLogoFile { get; set; }
        public string BannerLogoFile { get; set; }
        
        public Guid? CurAcdYear { get; set; }

        [ForeignKey("OrgId")]
        public Organization Org { get; set; }
        
        [ForeignKey("CurAcdYear")]
        public AcdYear AcdYr{ get; set; }
    }
    public class TimeTableSetting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid TTSID { get; set; }
        [Required]
        public Guid StnId { get; set; }
        [Required]
        public int WorkingDays { get; set; }
        [Required]
        public int WorkingHours { get; set; }
        [ForeignKey("StnId")]
        public Standard Std { get; set; }
    }
    public class TimeTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid TTID { get; set; }
        
        [Required]
        public int WeekNo { get; set; }
        [Required]
        public int HourNo { get; set; }
        [Required]
        public Guid StnSubMapId { get; set; }
        [Required]
        public Guid TeacherId { get; set; }

        [ForeignKey("TeacherId")]
        public Teacher Teach { get; set; }

        [ForeignKey("StnSubMapId")]
        public StdSubMap SSMap { get; set; }
    }

    public class AppLinkDistribution
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid ALID { get; set; }
        [Required]
        public Guid StuMapId { get; set; }
        [Required]
        public int Status { get; set; }
        public string Message { get; set; }
        [ForeignKey("StuMapId")]
        public StuStdAcdYearMap MapObj { get; set; }
    }

    public class NotificationRecp
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid RID { get; set; }
        [Required]
        public Guid NID { get; set; }
        [Required]
        public int RecpType { get; set; }
        public Guid RecpId { get; set; }
        [ForeignKey("NID")]
        public Notification Notf { get; set; }

    }
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid NID { get; set; }
        [Required]
        public Guid OrgId { get; set; }
        [Required]
        public DateTime NDate { get; set; }
        
        [Required]
        public Guid From { get; set; }
        [Required]
        public string NSubject { get; set; }
        [Required]
        public string NBody { get; set; }
        
        public string FileGuid { get; set; }
    }
    public class CoCurricularMark
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid CCID { get; set; }
        [Required]
        public Guid ExamId { get; set; }
        [Required]
        public Guid StuMapId { get; set; }
        [Required]
        public double ValueEducation { get; set; }
        [Required]
        public double ComputerEducation { get; set; }
        [Required]
        public double CulturalEducation { get; set; }
        [Required]
        public double PhysicalEducation { get; set; }
        [Required]
        public double OtherAreas { get; set; }
        [ForeignKey("ExamId")]
        public Exam Exm { get; set; }
        [ForeignKey("StuMapId")]
        public StuStdAcdYearMap SSAMap { get; set; }

    }
    public class GradeRange
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid GRID { get; set; }
        [Required]
        public Guid OrgId { get; set; }
        [Required]
        public string GradeText { get; set; }
        [Required]
        public double MinMarks { get; set; }
        [Required]
        public double MaxMarks { get; set; }
        [Required]
        public double GradePoint { get; set; }
        [ForeignKey("OrgId")]
        public Organization Org { get; set; }

    }

    public class ExamProgressReportHead
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid PRHId { get; set; }
        [Required]
        public Guid ExamId { get; set; }
        [Required]
        public string HeadName { get; set; }

        [Required]
        public double  MaxMarks { get; set; }
        [Required]
        public double MinMarks { get; set; }
        [ForeignKey("ExamId")]
        public Exam Exm { get; set; }

    }

    public class ExamProgressReport
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid PRId { get; set; }
        [Required]
        public Guid SchId { get; set; }
        [Required]
        public Guid StuMapId { get; set; }
        [Required]
        public Guid PRHeadId { get; set; }
        [Required]
        public double Marks { get; set; }
        public string SubwiseRemarks { get; set; }
        [ForeignKey("SchId")]
        public ExamSchedule ExmSchedule { get; set; }

        [ForeignKey("StuMapId")]
        public StuStdAcdYearMap SSAMap { get; set; }

        [ForeignKey("PRHeadId")]
        public ExamProgressReportHead PRHead { get; set; }
    }

    public class ChapterwiseExam
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid ExamId { get; set; }

        [Required]
        public DateTime ExamDate { get; set; }
        public string Notes { get; set; }
        [Required]
        public string FileName { get; set; }
        [Required]
        public string MimeType { get; set; }
    }

    public class ChapterwiseExamChapter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        [Required]
        public Guid ExamId { get; set; }
        [Required]
        public Guid ChapId { get; set; }
        [ForeignKey("ChapId")]
        public SubChapeter Chapter { get; set; }
        [ForeignKey("ExamId")]
        public ChapterwiseExam Exam { get; set; }
    }
    /// <summary>
    /// ExamId-MapId combination is UNIQUE.
    /// </summary>
    public class ExamSchedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid ExamSchId { get; set; }

        [Required]
        public Guid ExamId { get; set; }
        [Required]
        public Guid MapId { get; set; }//Standard & Subject Map Id
        [Required]
        public DateTime ExamDate { get; set; }
        
        public string Notes { get; set; }

        [Required]
        public int OrderNumber { get; set; } 
        [Required]
        public string FileName { get; set; }
        [Required]
        public string MimeType { get; set; }

        [ForeignKey("ExamId")]
        public Exam CurExam { get; set; }
        [ForeignKey("MapId")]
        public StdSubMap CurStnSubMap { get; set; }

    }

    public class Exam
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid ExamId { get; set; }

        [Required]
        public Guid ExamTypeId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [ForeignKey("ExamTypeId")]
        public ExamType EType { get; set; }
    }


    public class ExamType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid ExamTypeId { get; set; }
        [Required]
        public string TypeText { get; set; }
        [Required]
        public Guid OrgId { get; set; }

        [ForeignKey("OrgId")]
        public Organization Org { get; set; }

    }
    public class AssignmentSubmission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid AssnSubId { get; set; }
        [Required]
        public Guid AssnId { get; set; }

        [Required]
        public Guid StuMapId { get; set; }//Studdnt Map id

        [Required]
        [Column(TypeName = "date")]
        public DateTime SubDate { get; set; }

        [Required]
        public string AssnSubPath { get; set; }

        [Required]
        public string MimeType { get; set; }


        [ForeignKey("AssnId")]
        public Assignment Assmt { get; set; }
        [ForeignKey("StuMapId")]
        public StuStdAcdYearMap StudentMap { get; set; }
    }
    public class Assignment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid AssnId { get; set; }
        [Required]
        public Guid MapId { get; set; }//Standard-Subject MapId

        [Required]
        [Column(TypeName = "date")]
        public DateTime AssnDate { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string AssnFileName { get; set; }
        [Required]
        public string MimeType { get; set; }

        [ForeignKey("MapId")]
        public StdSubMap StdSub { get; set; }

    }
    public class HomeWorkSubmission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid HWSubId { get; set; }
        [Required]
        public Guid HWId { get; set; }

        [Required]
        public Guid StuMapId { get; set; }//Studdnt Map id

        [Required]
        [Column(TypeName = "date")]
        public DateTime SubDate { get; set; }

        [Required]
        public string HWSubPath { get; set; }
        [ForeignKey("HWId")]
        public HomeWork HW { get; set; }
        [ForeignKey("StuMapId")]
        public StuStdAcdYearMap StudentMap { get; set; }

    }
    public class HomeWork
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid HWId { get; set; }
        [Required]
        public Guid MapId { get; set; }//Standard-Subject MapId

        [Required]
        [Column(TypeName = "date")]
        public DateTime HWDate { get; set; }
        
        public string HomeWorkString { get; set; }

        public string ClassWorkString { get; set; }

        [ForeignKey("MapId")]
        public StdSubMap StdSub { get; set; }


    }
    public class DigitalContent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid DCId { get; set; }
        [Required]
        public Guid ChapId { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]
        public string ContentPath { get; set; }
        [Required]
        public int DCType { get; set; }//1: Image 2: PDF 3: Video 4: URL
        
        public string MimeType { get; set; }

        [Required]
        public int Status { get; set; }//1: Active 0:Inactive
        [ForeignKey("ChapId")]
        public SubChapeter Chap { get; set; }
    }
    public class OnlineSessionAttendee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid AtId { get; set; }
        [Required]
        public Guid OSId { get; set; }
        [Required]
        public DateTime EvtDate { get; set; }
        [Required]
        public Guid MapId { get; set; }
        [ForeignKey("OSId")]
        public OnLineSessionInfo OSInfo { get; set; }

        [ForeignKey("MapId")]
        public StuStdAcdYearMap StuMap{ get; set; }
    }
    public class OnLineSessionInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid OSId { get; set; }
        [Required]
        public Guid SubMapId { get; set; }//Standard Subject maps id
        
        [Required]
        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }

        [Required]
        [Column(TypeName = "time")]
        public TimeSpan StartTime { get; set; }

        [Required]
        [Column(TypeName = "time")]
        public TimeSpan EndTime { get; set; }

        [Required]
        public int IsReapated { get; set; }
        public string RepeatString { get; set; }

        [Required]
        public Guid OSessionUrlId { get; set; }

        [ForeignKey("SubMapId")]
        public StdSubMap SSMap { get; set; }

        [ForeignKey("OSessionUrlId")]
        public OnlineSessionUrl SUrl { get; set; }

    }
    public class OnlineSessionUrl
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid OSUrlId { get; set; }
        //public Guid OrgId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public Guid TeacherId { get; set; }
        [Required]
        public string SessionUrl { get; set; }
        [ForeignKey("TeacherId")]
        public Teacher Tchr { get; set; }
    }

    public class Chalan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid ChlId { get; set; }
        [Required]
        public string ChlnNumber { get; set; }
        [Required]
        public int SNo { get; set; }
        [Required]
        public Guid MapId { get; set; }
        [Required]
        public DateTime ChlDate { get; set; }

        public int ChalanStatus { get; set; }
        [ForeignKey("MapId")]
        public StuStdAcdYearMap SSMap { get; set; }
        
    }

    public class ChalanLineInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid ChlLineId { get; set; }
        [Required]
        public Guid ChlId { get; set; }
        [Required]
        public int TermNo { get; set; }
        [Required]
        public Guid FeeId { get; set; }
        [Required]
        public string FeeHeadName { get; set; }
        [Required]
        public double TotAmt { get; set; }
        [Required]
        public double PaidAmt { get; set; }
        [Required]
        public int DueMon { get; set; }
        [ForeignKey("ChlId")]
        public Chalan Chln { get; set; }
        [ForeignKey("FeeId")]
        public FeeMaster Fee { get; set; }
    }
    public class FeeConcession
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid ConId { get; set; }
        public Guid FeeId { get; set; }
        public Guid MapId { get; set; }
        public double Amount { get; set; }

        [Required]
        public int ConcessionType { get; set; }

        [Required]
        public string Reason { get; set; }
        [ForeignKey("FeeId")]
        FeeMaster FId { get; set; }
        [ForeignKey("MapId")]
        public StuStdAcdYearMap SSMap { get; set; }
    }
    public class FeeHeadMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid FeeHeadId { get; set; }
        
        [Required]
        public Guid OrgId { get; set; }
        
        [Required]
        public string FeeHeadName { get; set; }

        [Required]
        public int FeeType { get; set; }
        
        [Required]
        public int Terms { get; set; }

        [ForeignKey("OrgId")]
        public Organization Org { get; set; }

    }

    public class FeeMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid FeeId { get; set; }

        [Required]
        public Guid FHeadId { get; set; }

        [Required]
        public int TermNo { get; set; }

        public Guid? StnId { get; set; }//Standard Id
        public Guid? MapId { get; set; }//Student-Standard map Id

        [Required]
        public double Amount { get; set; }

        [Required]
        public Guid AcdyearId { get; set; }

        [Required]
        public int DueDayNo { get; set; }

        [Required]
        public int DueMonthNo { get; set; }

        [ForeignKey("StnId")]
        public Standard Stnd { get; set; }

        [ForeignKey("MapId")]
        public StuStdAcdYearMap SSMap { get; set; }

        [ForeignKey("AcdyearId")]
        public AcdYear AcdYear { get; set; }

    }

    public class FeeCollection
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid FeeColId { get; set; }

        [Required]
        public Guid MapId { get; set; }

        [Required]
        public Guid ChlnId { get; set; }

        [Required]        
        public int PayType { get; set; }

        public string Notes { get; set; } 

        [Required]
        [Column(TypeName = "date")]
        public DateTime ColDate { get; set; }

        [ForeignKey("MapId")]
        public StuStdAcdYearMap SSMap { get; set; }

        [ForeignKey("ChlnId")]
        public Chalan Chln { get; set; }

    }

    public class FeeCollectionLineItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid LineItemId { get; set; }
        [Required]
        public Guid ColId { get; set; }
        [Required]
        public Guid FeeId { get; set; }

        [Required]
        public double Amount { get; set; }

        [ForeignKey("ColId")]
        public FeeCollection FC { get; set; }

        [ForeignKey("FeeId")]
        public FeeMaster FM { get; set; }

    }
    public class Teacher
    { 
        [Key]        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid TeacherId { get; set; }

        [Required]
        public Guid OrgId { get; set; }

        [Required]
        public string EmpId { get; set; }
        
        [Required]
        public string FName { get; set; }

        
        public string MName { get; set; }
        public string LName { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime DOJoining { get; set; }

        public string Address { get; set; }
        public string MobileNo { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string Gender { get; set; }
        public string BllodGroup { get; set; }
        public string TeacherType { get; set; }//Primary, PrePrimary, Secondary, higher Sr.

        public int Status { get; set; }
        public Guid? LoginUID { get; set; }


        [ForeignKey("OrgId")]
        public Organization Org { get; set; }

        [ForeignKey("LoginUID")]
        public UserInfo UInfo { get; set; }
    }
    public class Cashier
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid CashierId { get; set; }

        [Required]
        public Guid OrgId { get; set; }

        [Required]
        public string EmpId { get; set; }

        [Required]
        public string FName { get; set; }


        public string MName { get; set; }
        public string LName { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime DOJoining { get; set; }

        public string Address { get; set; }
        //public string MobileNo { get; set; }
        public int Status { get; set; }
        public Guid? LoginUID { get; set; }
        
        [ForeignKey("OrgId")]
        public Organization Org { get; set; }

        [ForeignKey("LoginUID")]
        public UserInfo UInfo { get; set; }
    }
    public class AcdYear
    {   
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid AcdId { get; set; }

        [Required]
        public string AcdText { get; set; }

    }

    public class StudentInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid StuId { get; set; }
        
        [Required]
        public string RegdNo { get; set; }

        [Required]
        public string FName { get; set; }

        public string MName { get; set; }
        public string LName { get; set; }


        [Required]
        [Column(TypeName = "date")]
        public DateTime DOBirth { get; set; }


        [Required]
        [Column(TypeName = "date")]
        public DateTime DOAdmission { get; set; }

        public string ResAddress { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string FatherMobile { get; set; }
        public string Gender { get; set; }
        public string BloodGroup { get; set; }
        public string ParentEmail { get; set; }
        public string AadharNo { get; set; }
        public string Religion { get; set; }
        public string Cast { get; set; }
        public string SchoolAdmNo { get; set; }
        public int IsActive { get; set; }
        
        public Guid? LoginUID { get; set; }

        //Abosoate Fields..... Need to remove
        /*public Guid? StuImageFile { get; set; }

        public Guid? StuFatherImageFile { get; set; }

        public Guid? StuMotherImageFile { get; set; }*/
        //End Of Absolate fields

        public byte[] StudentImage { get; set; }
        public string StudentImageMimeType { get; set; }

        public byte[] StuFatherImage { get; set; }
        public string StuFatherImageMimeType { get; set; }
        public byte[] StuMotherImage { get; set; }
        public string StuMotherImageMimeType { get; set; }

        [ForeignKey("LoginUID")]
        public UserInfo UInfo { get; set; }

    }

    public class StuStdAcdYearMap
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid MapId { get; set; }

        [Required]
        public Guid AcYearId { get; set; }
        
        [Required]
        public Guid StnId { get; set; }
        
        [Required]
        public Guid StuId { get; set; }

        [ForeignKey("AcYearId")]
        public AcdYear AcYear { get; set; }

        [ForeignKey("StnId")]
        public Standard Stn { get; set; }

        [ForeignKey("StuId")]
        public StudentInfo Stu { get; set; }

        [Required]
        public int RecType { get; set; }//1: Promoation, 0: New Join
        
        [Required]
        [Column(TypeName = "date")]
        public DateTime RecDate { get; set; }//Date Of row creation.

    }

    public class SubChapeter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid ChapId { get; set; }

        [Required]
        public Guid MapId { set; get; }

        [Required]
        public string ChapName { get; set; }

        [ForeignKey("MapId")]
        public StdSubMap StdSubMp { get; set; }

    }

    public class StdSubMap
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid MapId { get; set; }

        //[Required]
        public Guid? StdId { get; set; }

        //[Required]
        public Guid? SubId { get; set; }

        [ForeignKey("StdId")]
        public Standard Std { get; set; }


        [ForeignKey("SubId")]
        public Subject Sub { get; set; }

    }
    public class Standard
    { 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid StdId { get; set; }

        [Required]
        public Guid OrgId { get; set; }

        [Required]
        public string StdName { get; set; }

        [ForeignKey("OrgId")]
        public Organization Org { get; set; }
    }

    public class Subject
    { 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid SubId { get; set; }

        [Required]
        public Guid OrgId { get; set; }

        [Required]
        public string SubjectName { get; set; }
        [Required]
        public string SubCode { get; set; }

        [ForeignKey("OrgId")]
        public Organization Org { get; set; }
        public int? IsLanguage { get; set; }
        public int? LangOrdinal { get; set; }
    }

    public class Enquiry
    { 
        [Key]
        public long ID { get; set; }
        [Required]
        public string SchoolName { get; set; }
        [Required]
        public string ContactName { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Email { get; set; }

        [Required]
        [Column(TypeName="date")]
        public DateTime EnqDate { get; set; }
        
        [Required]
        public string Status { get; set; }
    }
    public class Organization
    { 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid OrgId { get; set; }
        [Required]
        public string OrgCode { get; set; }
        [Required]
        public string OrgName { get; set; }
        [Required]
        public string OrgAddress { get; set; }
        [Required]
        public string OrgPOC { get; set; }

        [Required]
        public string OrgEmail { get; set; }
        [Required]
        public string OrgMobile { get; set; }

    }
    public class RoleMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid RoleID { get; set; }
        [Required]
        public int RoleVal { get; set; }
        [Required]
        public string RoleName { get; set; }
    }


    public class UserInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid ID { get; set; }
        [Required]
        public string UserID { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string MobNo { get; set; }
        
        [Required]
        public string Emailid { get; set; }
        public int? IsSiteAdmin { get; set; }
    }

    public class UserOrgMap
    { 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid MapId { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid OrgId { get; set; }
        [Required]
        public Guid RoleId { get; set; }

        [ForeignKey("UserId")]
        public UserInfo Uinfo { get; set; }

        [ForeignKey("OrgId")]
        public Organization OrgInfo { get; set; }

        [ForeignKey("RoleId")]

        public RoleMaster RoleInfo { get; set; }
    }

    public class LanguageMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid LangId { get; set; }

        [Required]
        public Guid OrgId { get; set; }

        [Required]
        public string LanguageName { get; set; }

        [ForeignKey("OrgId")]
        public Organization Org { get; set; }
    }

    public class StudentLangMap
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid MapId { get; set; }
        [Required]
        public Guid StudentStnAcdMapId { get; set; }
        [Required]
        public Guid LanguageId { get; set; }
        [Required]
        public int LangOrdinal { get; set; }

        [ForeignKey("StudentStnAcdMapId")]
        public virtual StuStdAcdYearMap StudentStnAcdMapEntry { get; set; }

        [ForeignKey("LanguageId")]
        public virtual LanguageMaster Language { get; set; }

    }
}
