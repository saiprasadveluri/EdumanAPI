using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EduManAPI
{
    public class FileManagerDTO
    {
        public byte[] Data { get; set; }
        public string Name { get; set; }
        public string MimeType { get; set; }
    }
    public class FileManagerInputDTO
    {
        public int ReqDataIdentifer { get; set; }
        public string UniqIdentifer { get; set; }
        public IFormFile InpFile { get; set; }
    }
    public class StandardLanguageDTO
    {
        public int LangOrdNumber { get; set; }
        public Guid LanguageId { get; set; }
    }
    public class AddStudentLanguageMapDTO
    {
        public int LangOrdinal { get; set; }
        public Guid StuMapId { get; set; }
        public Guid LanguageId { get; set; }
    }
    public class StudentLangMapDTO
    {
        public Guid MapId { get; set; }        
        public Guid? StudentId { get; set; }  
        public int LangOrdinal { get; set; }
        public Guid? LanguageId { get; set; }
        public string LanguageName { get; set; }
    }

    public class LanguageMasterDTO
    {
        public Guid LangId { get; set; }
        public Guid OrgId { get; set; }
        public string LanguageName { get; set; }
    }
    public class TempExpensereportEntry 
    {
        public Guid HeadId { get; set; }
        public string ExpHeadName { get; set; }
        public double TotalAmount { get; set; }
        public int DiscountType { get; set; }
        public double DiscountValue { get; set; }
    }

    public class ExpensereportEntry
    {
        public Guid HeadId { get; set; }
        public string ExpHeadName { get; set; }
        public double TotalAmount { get; set; }
        public double TotalDsicount { get; set; }
    }
    public class PettyExpnseDTO
    {
        public Guid PEID { get; set; }
        public string Title { get; set; }
        public Guid OrgId { get; set; }
        public Guid EHeadID { get; set; }
        public string EHeadName { get; set; }
        public string PaymentMode { get; set; }
        public double Amount { get; set; }
        public Guid AccountTypeID { get; set; }
        public string AccountTypeName { get; set; }
        public string ChequeOrRefNo { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaidTo { get; set; }
        public string Ms { get; set; }
        public string Comments { get; set; }
        public string FileGuid { get; set; }
        public string FileBase64Content { get; set; }
        public string FileMIME { get; set; }
    }
    public class BillDTO
    {
        
        public Guid BID { get; set; }
        
        public Guid OrgId { get; set; }
        
        public Guid VendorId { get; set; }
        public string VendorName { get; set; }
        public Guid ExpHeadId { get; set; }
        public string ExpHeadName { get; set; }
        public DateTime BillDate { get; set; }
        public DateTime DueDate { get; set; }
        public string BillNumber { get; set; }
        //public string FileGuid { get; set; }
        public string FileBase64Content { get; set; }
        public string FileMIME { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }
        public int DiscountType { get; set; }//1: Amount, 2: Percentage
        public double DiscountVal { get; set; }
    }

    public class BillAttachmentDTO
    { 
        public string Base64Content { get; set; }
        public string MimeType { get; set; }
    }
    public class BillLineItemDTO
    {
        public Guid BLID { get; set; }
        public Guid BID { get; set; }
        public Guid ProdId { get; set; }
        public string ProdName { get; set; }
        public Guid TaxId { get; set; }
        public string TaxName { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
    }

    public class ServiceOrProductDTO
    {        
        public Guid PID { get; set; }
        public Guid OrgId { get; set; }
        public Guid EHId { get; set; }
        public string EHName { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public Guid TID { get; set; }
        public string TaxName { get; set; }
    }

    public class TaxDTO
    {
        public Guid TID { get; set; }
        public Guid OrgId { get; set; }
        public string Title { get; set; }
        public double Percentage { get; set; }
        public string TaxNumber { get; set; }
        public string Description { get; set; }
    }
    public class BankDeatailDTO
    {
        public Guid BID { get; set; }
        public Guid OrgId { get; set; }
        public int AccountType { get; set; }
        public string CashierName { get; set; }
        public string BankName { get; set; }
        public string BankAccountNo { get; set; }
        public string WalletName { get; set; }
        public string MobileNo { get; set; }
        public int Status { get; set; }
        
    }

    public class ExpenseHeadDTO
    {
        public Guid EHID { get; set; }
        public Guid OrgId { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public string Note { get; set; }
    }

    public class VendorListEntryDTO
    {
        public Guid VendorId { get; set; }
        public Guid OrgId { get; set; }
        public string VendorName { get; set; }
        public string ContactName { get; set; }
        public string ContactNo { get; set; }
        public string Location { get; set; }
        public string Address { get; set; }
        public int Status { get; set; }
    }
    public class EmployeeSalDTO
    {   public Guid SalId { get; set; }
        public int MonthNo { get; set; }
        public string Year { get; set; }
        public List<EmployeeSalEntryDTO> lines { get; set; } = new List<EmployeeSalEntryDTO>();
    }

    public class EmployeeSalEntryDTO
    { 
      public Guid SalDetId { get; set; }
      public string HeadName { get; set; }
      public int HeadType { get; set; }
      public double Amount { get; set; }
    }

    public class ActOnSalaryDTO
    {
        public string chks { get; set; }
        public int ActionToBeTaken { get; set; }//0: Delete 1: Approve

    }
    public class SalaryListEntryDTO
    {
        public Guid SALID { get; set; }
        public Guid EmpId { get; set; }
        public string EmpName { get; set; }
        public int MonthNo { get; set; }
        public string Year { get; set; }
        public int Status { get; set; }//0: Pending 1: Approved

    }
    public class EmployeeWithPayrollListDTO
    { 
        public Guid EmpGUID { get; set; }
        public string EmpName { get; set; }

    }
    public class PayrollDTO
    {
        public Guid PRID { get; set; }
        public Guid HID { get; set; }
        public Guid EmpID { get; set; }
        public double OrgAmount { get; set; }
        public int Status { get; set; }
    }


    public class GetPayrollDTO
    {
        public Guid HID { get; set; }
        public string PHName { get; set; }
        public double OrgAmount { get; set; }
        public int Status { get; set; }
        public int PHType { get; set; }
    }

    public class PayheadDTO
    {
        public Guid PHID { get; set; }
        public Guid OrgId { get; set; }
        public string Name { get; set; }
        public int HeadType { get; set; }
    }
    public class CashierDTO
    {
        public Guid CashierId { get; set; }
        public Guid OrgId { get; set; }
        public string EmpId { get; set; }
        public string FName { get; set; }
        public string MName { get; set; }
        public string LName { get; set; }
        public DateTime DOJoining { get; set; }
        public string Address { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public int Status { get; set; }
        //public Guid? LoginUID { get; set; }
    }

    public class FeeDefaulterInfoDTO
    {
        public string StuName { get; set; }
        public string RegdNo { get; set; }
        public string Stn { get; set; }
        public double Amount{ get; set; }
        public string HeadName { get; set; }
        public int Term { get; set; }
        public double FeeAmt { get; set; }
        public double Concession { get; set; }
    }
    public class SchoolPrintBannerDTO
    { 
       public Guid OrgId { get; set; }
        public string BannerContent { get; set; }
       public Guid AcdYearId { get; set; }
        public string AcdYearText { get; set; }
    }

    public class TimeTableSettingDTO
    {
        public Guid TTSID { get; set; }
        
        public Guid StnId { get; set; }
        public string StnString { get; set; }
        public int WorkingDays { get; set; }
        
        public int WorkingHours { get; set; }
        
    }

    public class ViewTeacherTimeTableEntryDTO
    {
        public int WeekNo { get; set; }
        public int HourNo { get; set; }
        public string SubName { get; set; }
        public string StnName { get; set; }
    }

    public class ViewTimeTableEntryDTO
    {
        public int WeekNo { get; set; }
        public int HourNo { get; set; }
        public string SubName { get; set; }
        public string TeacherName { get; set; }
    }

    public class AddTimeTableDTO
    {
        public Guid OrgId { get; set; }
        public Guid StnId { get; set; }
        public List<AddTimeTableEntryDTO> entries { get; set; } = new List<AddTimeTableEntryDTO>();
    }
    public class AddTimeTableEntryDTO
    { 
        public int WeekNo { get; set; }
        public int HourNo { get; set; }
        public string SubCode { get; set; }
        public string TeacherEmpId { get; set; }
    }
    public class AppLinkDistributionEditEntryDTO
    {
        public Guid StuMapId { get; set; }
    }
    public class AppLinkDistributionEntryDTO
    {
        public Guid ALID { get; set; }
        
        public Guid StuMapId { get; set; }
        public string StuName { get; set; }
        public string RegdNo { get; set; }
        
        public int Status { get; set; }
        public string Message { get; set; }
    }
    public class HomeWorkSubmissionAddDTO
    {
        public Guid HWId { get; set; }
        public Guid StuMapId { get; set; }//Student Map id
        public string FileContentBase64 { get; set; }
    }

    public class StudentDetailsDTO
    { 
        public Guid ID { get; set; }
        public Guid StnID { get; set; }
        public Guid MapId { get; set; }
        public string Name { get; set; }
        public string StandardName { get; set; }
    }
    public class LoginDTO
    {
        public string UserID { get; set; }
        public string Password { get; set; }
        public Guid? SelOrgId { get; set; }
        public bool? GenToken { get; set; }
    }

    public class UserOrgInfo
    {
        public Guid OrgId { get; set; }
        public string OrgName { get; set; }
    }
    
    public class LoginDataInfoDTO
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        
    }
    public class NewUserInfo
    {
        public Guid ID { get; set; }
        public Guid OrgId { get; set; }
        public string OrgCode { get; set; }
        public string OrgName { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string Status { get; set; }
        public Guid RoleId { get; set; }
        public int RoleVal { get; set; }
        public string RoleString { get; set; }  
        public string JwtTokenString { get; set; }
        public int? IsSiteAdmin { get; set; }
        public List<UserOrgMapDTO> UserOrgMapDTOs { get; set; } = new List<UserOrgMapDTO>();
    }

    public class NotificationViewDTO
    {
        public Guid NID { get; set; }
       
        public DateTime NDate { get; set; }
        public string From { get; set; }
        public string NSubject { get; set; }
        public string NBody { get; set; }
        public string ToLst { get; set; }        
    }

    public class NotificationSentDTO
    {
        public Guid NID { get; set; }

        public DateTime NDate { get; set; }
        public string From { get; set; }
        public string NSubject { get; set; }
        public string NBody { get; set; }
        public string ToLst { get; set; }
    }

    public class NotificationSentTempDTO
    {
        public Guid NID { get; set; }
        public DateTime NDate { get; set; }
        public string From { get; set; }
        public string NSubject { get; set; }
        public string NBody { get; set; }
        public int RecpType { get; set; }
        public Guid RecId { get; set; }
    }
    public class NotificationDTO
    {   
        public Guid NID { get; set; }
        public Guid OrgId { get; set; }
        public DateTime NDate { get; set; }
        public Guid From { get; set; }
        public string NSubject { get; set; }
        public string NBody { get; set; }
        public string FileBase64String { get; set; }
        public List<NotificationRecpDTO> Recps { get; set; } = new List<NotificationRecpDTO>();
    }

    public class NotificationRecpDTO
    {   
        public Guid RID { get; set; }
        public Guid NID { get; set; }
        public int RecpType { get; set; }
        public Guid RecpId { get; set; }
    }
    public class SelectElementDataDTO
    { 
        public string ID { get; set; } 
        public string name { get; set; }
    }
   
    public class ClasswiseProgressReportDTO
    {
        public List<IndividualProgressReportDTO> lst { get; set; } = new List<IndividualProgressReportDTO>();
    }
    public class CoCurricularMarkDTO
    {
        public Guid CCID { get; set; }
        public Guid ExamId { get; set; }
        public Guid StuMapId { get; set; }
        public double ValueEducation { get; set; }
        public double ComputerEducation { get; set; }
        public double CulturalEducation { get; set; }
        public double PhysicalEducation { get; set; }
        public double OtherAreas { get; set; }
    }

    public class GradeRangeDTO
    {
        public Guid GRID { get; set; }
        public Guid OrgId { get; set; }
        public string GradeText { get; set; }
        public double MinMarks { get; set; }
        public double MaxMarks { get; set; }
        public double GradePoint { get; set; }
    }

    public class IndividualProgressReportDTO
    {
        public string RegdNo { get; set; }
        public string StuName { get; set; }
        public string StnName { get; set; }
        public List<IndividualProgressReportSubject> Subjects { get; set; } = new List<IndividualProgressReportSubject>();
    }

    public class IndividualProgressReportSubject
    { 
        public string SubjectName { get; set; }
        public List<IndividualProgressReportSubjectMark> SubMarks { get; set; } = new List<IndividualProgressReportSubjectMark>();
    }

    public class IndividualProgressReportSubjectMark
    {
        public string HeadName { get; set; }
        public double Marks { get; set; }
        public double Maxmarks { get; set; }
        public double Minmarks { get; set; }
    }

    public class ExamProgressReportEntryDTO
    {
        public Guid PRId { get; set; }
        public Guid SchId { get; set; }
        public Guid StuMapId { get; set; }
        public Guid PRHeadId { get; set; }
        public double Marks { get; set; }
        //public string SubwiseRemarks { get; set; }
        
    }

    public class ProgressReportHeadDTO
    {
        public Guid PRHId { get; set; }
        public Guid ExamId { get; set; }
        public string HeadName { get; set; }
        public double MaxMarks { get; set; }
        public double MinMarks { get; set; }
    }
    public class AtachmentGetDTO
    {
        public string MimeType { get; set; }
        public string FileBase64String { get; set; }
    }

    public class ExamScheduleAtachmentGetDTO
    {
        public string MimeType { get; set; }
        public string FileBase64String { get; set; }
    }
    public class ChapterwiseExamViewDTO
    {
        public Guid ExamId { get; set; }
        public DateTime ExamDate { get; set; }
        public string Notes { get; set; }
    }
    public class ChapterwiseExamViewEntryDTO
    {
        public Guid ChapId { get; set; }
        public string ChapName { get; set; }
        public string StnName { get; set; }
        public string SubjectName { get; set; }        
    }

    public class ChaperExamAddDTO
    {
        public Guid ExamId { get; set; }
        public DateTime ExamDate { get; set; }
        public string Notes { get; set; }
        public string FileContentBase64 { get; set; }
        public string MType { get; set; }
        public List<ChaperExamEntryAddDTO> entries { get; set; } = new List<ChaperExamEntryAddDTO>();
    }

    public class ChaperExamEntryAddDTO
    {
        public Guid ChapId { get; set; }
    }

    public class ExamScheduleAddDTO
    { 
        public Guid ExamId { get; set; }
        public Guid ExamTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<ExamScheduleEntryAddDTO> entries { get; set; } = new List<ExamScheduleEntryAddDTO>();
    }

    public class ExamScheduleEntryAddDTO
    {
        public Guid MapId { get; set; }
        public DateTime ExamDate { get; set; }
        public string Notes { get; set; }
        public int OrdNo { get; set; }
        public string FileBase64String { get; set; }
        public string MimeType { get; set; }
    }

    public class ExamViewDTO
    {
        public Guid ExamId { get; set; }
        public Guid ExamTypeId { get; set; }
        public string ExamTypeString { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
       // public List<ExamScheduleViewEntryDTO> entries { get; set; } = new List<ExamScheduleViewEntryDTO>();
    }
    public class ExamScheduleViewEntryDTO
    {
        public Guid ExmSchId{get;set;}
        public Guid MapId { get; set; }
        public Guid StnId { get; set; }
        public string StnName { get; set; }
        public string SubjectName { get; set; }
        public DateTime ExamDate { get; set; }
        public string Notes { get; set; }
        public int OrdNo { get; set; }
    }

    public class ExamTypeDTO
    {
        public Guid ExamTypeId { get; set; }
        
        public string TypeText { get; set; }
        
        public Guid OrgId { get; set; }

    }

    public class AssnSubmissionDownloadDTO
    { 
        public string FileBase64Str { get; set; }
        public string MimeType { get; set; }
    }
    public class AssignmentSubmissionDTO
    {
        public Guid AssmSubId { get; set; }
        public string StnName { get; set; }
        public string SubName { get; set; }
        public string StudentName { get; set; }
        public string RegdNo { get; set; }

        public DateTime SubDate { get; set; }
    }

    public class AssignmentAtachmentGetDTO
    {
      public string MimeType { get; set; }
      public string FileBase64String { get; set; }
    }
    public class AssignmentAddDTO
    {
        public Guid MapId { get; set; }//Standard-Subject MapId

        public DateTime AssnDate { get; set; }

        public string Title { get; set; }

        public string FileBase64String { get; set; }

        public string MimeType { get; set; }
    }

    public class AssignmentGetListEntryDTO
    {        
        public Guid AssnId { get; set; }
        public string StnName { get; set; }
        public string SubName { get; set; }
        public string AssnDate { get; set; }
        public string Title { get; set; }
    }

    public class HomeWorkSubmissionDTO
    {
        public Guid HWSubId { get; set; }        
        public string StnName { get; set; }
        public string SubName { get; set; }
        public string StudentName { get; set; }
        public string RegdNo { get; set; }

        public DateTime SubDate { get; set; }
    }

    public class HomeWorkGetListEntryDTO
    {        
        public Guid HWId { get; set; }
        public string StnName { get; set; }
        public string SubName { get; set; }
        public string HWDate { get; set; }
        public string HomeWorkString { get; set; }
        public string ClassWorkString { get; set; }
    }

    public class HomeWorkAddDTO
    {
        public Guid MapId { get; set; }//Standard-Subject MapId

        public DateTime HWDate { get; set; }

        public string HomeWorkString { get; set; }

        public string ClassWorkString { get; set; }

        
    }

    public class FeeReceiptInfoDTO
    { 
        public Guid FeeColId { get; set; }
        public DateTime ColDate { get; set; }
        public string StuRegdNo { get; set; }
        public string StuName { get; set; }
        public string Standard { get; set; }
        public int PayType { get; set; }
        public double Amount { get; set; }
        public string ChlnNumber { get; set; }
    }
    public class FeeConcessionDTO
    { 
        public Guid ConId { get; set; }
        public Guid FeeId { get; set; }
        public Guid MapId { get; set; }
        public double Amt { get; set; }
        public string Reason { get; set; }
        public int ConcessionType { get; set; }
    }

    public class FeeConcessionViewDTO
    {
        public Guid ConId { get; set; }
        public string FeeHeadName { get; set; }
        public double Amt { get; set; }
        public string Reason { get; set; }
        public int TermNo { get; set; }
        public int ConcessionType { get; set; }
    }


    public class FeeCollectionReceiptDTO
    { 
        public string ChlnNumber { get; set; }
        public int SuccessFalg { get; set; }
        public string RegdNo { get; set; }
        public string Name { get; set; }
        public int PayType { get; set; }
        public DateTime Paydate { get; set; }
        public string Notes { get; set; }
        public List<FeeCollectionReceiptLineDTO> lines { get; set; } = new List<FeeCollectionReceiptLineDTO>();
    }

    public class FeeCollectionReceiptLineDTO
    {
        public string FeeHeadName { get; set; }
        public int TermNo { get; set; }
        public double Amt { get; set; }
    }
    public class DigitalContentAddDTO
    {
        public Guid DCId { get; set; }
        public Guid ChapId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int DCType { get; set; }//1: Image 2: PDF 3: Video 4: URL
        public string MimeType { get; set; }
        public int Status { get; set; }//1: Active 0:Inactive
       
    }

    public class DigitalContentDTO
    {
        public Guid DCId { get; set; }
        public string Standard { get; set; }
        public string Subject { get; set; }
        public string Chapter { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int DCType { get; set; }//1: Image 2: PDF 3: Video 4: URL
        public string MimeType { get; set; }
        public int Status { get; set; }//1: Active 0:Inactive

    }
    public class OnlineSessionAttendeeEntryDTO
    {
        public Guid MapId { get; set; }
        public string StudentName { get; set; }
        public int IsPresent { get; set; }
    }

    public class OnlineSessionAttendeeActionDTO
    { 
        public Guid OsId { get; set; }
        public DateTime EvtDate { get; set; }
        public Guid MapId { get; set; }
        public int ActionType { get; set; }//1: Mark Present 0: Mark Abscent.
    }
    public class OnLineSessionInfoDTO
    {
        
        public Guid OSId { get; set; }

        public string Standard { get; set; }
        public string StdId { get; set; }
        public DateTime EvtDate { get; set; }
        public string Subject { get; set; }
        public string TeachName { get; set; }
        public Guid SubMapId { get; set; }//Standard Subject maps id

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        /*public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }*/
        public string StartTime { get; set; }
        public string EndTime { get; set; }


        public int IsReapated { get; set; }
        public string RepeatString { get; set; }
        public string SesUrl { get; set; }
        public Guid OSessionUrlId { get; set; }
        public string OSessionUrl { get; set; }
    }

    public class OnlineSessionUrlDTO
    {   
        public Guid OSUrlId { get; set; }
        public string Title { get; set; }
        public Guid TeacherId { get; set; }
        public string TeacherName { get; set; }
        public string SessionUrl { get; set; }     
    }

    public class StudentFeeInfoDTO
    {
        public Guid MapId { get; set; }
        public string RegdNo { get; set; }
        public string Name { get; set; }
        public string Stndardname { get; set; }
        public List<StudentFeeInfoLineItem> Lines { get; set; } = new List<StudentFeeInfoLineItem>();
    }
    public class StudentFeeInfoLineItem
    {
        public Guid FID { get; set; }
        public string HN { get; set; }
        public int TermNo { get; set; }
        public double TotAmt { get; set; }
        public double ConAmout { get; set; }
    }
    public class ChalanListDTO
    {
        public Guid ChlId { get; set; }
        public DateTime ChlDate { get; set; }
        public string ChlnNum { get; set; }
    }

    public class StudentChalansInfoDTO
    {
        public Guid StuMapId { get; set; }
        public List<StudentChalansInfoLineItemDTO> Items { get; set; } = new List<StudentChalansInfoLineItemDTO>();
    }

    public class StudentChalansInfoLineItemDTO
    { 
        public Guid FId { get; set; }
        public double Amt { get; set; }
    }

    public class ChalanDTO
    {
        public string ChlnNum { get; set; }
        public Guid StdId { get; set; }
        public string AcdYear { get; set; }
        public Guid MapId { get; set; }
        public string RegdNo { get; set; }
        public string Name { get; set; }
        public string Stndardname{get;set;}
        public List<ChalanInfoDTO> info { get; set; } = new List<ChalanInfoDTO>();
    }
    public class ChalanInfoDTO
    {
        public Guid FID { get; set; }
        public string HN { get; set; }
        public int TermNo { get; set; }
        public double TotAmt { get; set; }
        public double Concession{ get; set; }
        public double Paid { get; set; }
        public double Due { get; set; }
        public int DueMon { get; set; }
    }
    
    public class FeeCollectionInfoDTO
    { 
        public Guid ColId { get; set; }
        public int PayType { get; set; }
        public DateTime ColDate { get; set; }
        public string Notes { get; set; }
        //public List<FeeCollectionInfoLineDTO> LineLst { get; set; }=new List<FeeCollectionInfoLineDTO>();
    }

    public class FeeCollectionInfoLineDTO
    { 
        public string HeadName { get; set; }
        public double Amount { get; set; }
        public int TermNo { get; set; }
    }

    public class FeeCollectionDTO_Absolate
    { 
        public List<Guid> FeeIdLst { get; set; }
        public Guid MapId { get; set; }
        public int PayType { get; set; }
        public DateTime ColDate { get; set; }
        public string Notes { get; set; }
        public double Amount { get; set; }
    }
    public class FeeChalanCollectionDTO
    {
        public Guid ChlnId { get; set; }
        
        public int PayType { get; set; }
        public DateTime ColDate { get; set; }
        public string Notes { get; set; }
        
    }


    public class FeeArrearDTO
    {
        public Guid FeeId { get; set; }
        public string HeadName { get; set; }
        public double Amount { get; set; }
        public int TermNo { get; set; }
    }

    public class FeeMasterDTO
    {
        public Guid FeeId { get; set; }

       
        public Guid FHeadId { get; set; }

       
        public int TermNo { get; set; }

        public Guid? StnId { get; set; }//Standard Id
        public string StnText { get; set; }
        public Guid? MapId { get; set; }//Student-Standard map Id
        public string StudentDispName { get; set; }
        
        public double Amount { get; set; }

        
        public Guid AcdyearId { get; set; }
        public string AcdYearText { get; set; }
        
        public int DueDayNo { get; set; }
                
        public int DueMonthNo { get; set; }
        
        //Extra param...
        public int AddMode { get; set; }
    }

    public class FeeHeadMasterDTO
    {
        
        public Guid FeeHeadId { get; set; }
        public Guid OrgId { get; set; }
        public string FeeHeadName { get; set; }
        public int FeeType { get; set; }
        public int Terms { get; set; }
    }


    public class AddTeacherDTO
    {
        public Guid TeacherId { get; set; }
        public Guid OrgId { get; set; }
        public string EmpId { get; set; }
        public string FName { get; set; }
        public string MName { get; set; }
        public string LName { get; set; }
        public DateTime DOJoining { get; set; }
        public string Address { get; set; }
        public string MobileNo { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string Gender { get; set; }
        public string BllodGroup { get; set; }
        public string TeacherType { get; set; }//Primary, PrePrimary, Secondary, higher Sr.
        public int Status { get; set; }
    }
    public class StudentPromotionDataDTO
    {
        public List<StudentPromotionDataEntryDTO> PromLst { get; set; } = new List<StudentPromotionDataEntryDTO>();
    }

    public class StudentPromotionDataEntryDTO
    {
        public Guid MapId { get; set; }
        public Guid StuId { get; set; }
        public string StuName { get; set; }
        public Guid StnId { get; set; }
        public string StnName { get; set; }
        public Guid AcdYearId { get; set; }
        public string AcdYearName { get; set; }
        public DateTime PromDate { get; set; }
    }


    public class StudentPromotionDTO
    {   
        public Guid ToAcdYearId { get; set; }
        public Guid ToStnId { get; set; }
        public List<Guid> StuIdLst { get; set; } = new List<Guid>();
    }
    public class AcdYearDTO
    {
        public Guid AcdId { get; set; }
        public string AcdText { get; set; }

    }

    public class GetStudentInfoReqDTO
    { 
        public Guid AyearId { get; set; }
        public Guid StdId { get; set; }
    }
    public class StuStdMapDTO
    { 
        public Guid MapId { get; set; }
        public string StudentName { get; set; }
        public Guid StudentId { get; set; } 
        public Guid StandardId { get; set; }
    }
    public class StudentInfoDTO
    {
        public Guid MapId { get; set; }
        public Guid StuId { get; set; }

        
        public string RegdNo { get; set; }

        
        public string FName { get; set; }

        public string MName { get; set; }
        public string LName { get; set; }


       
        public DateTime DOBirth { get; set; }


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

        
    }

    public class AddStudentInfoDTO
    {
        public Guid AcYearId { get; set; }
        public Guid StnId { get; set; }
        public Guid StuId { get; set; }
        public string RegdNo { get; set; }
        public string FName { get; set; }
        public string MName { get; set; }
        public string LName { get; set; }
        public DateTime DOBirth { get; set; }
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
        
        public Guid OrgId { get; set; }
        public List<StudentLanguageDetailsEntry> StuLangs { get; set; } = new List<StudentLanguageDetailsEntry>();
        
    }

    public class StudentLanguageDetailsEntry
    {
        public Guid LangId { get; set; }
        public int OrdinalNumber { get; set; }
    }
    public class BulkAddStudentInfoDTO
    {
        public Guid AcYearId { get; set; }
        public Guid StnId { get; set; }
        public List<AddStudentInfoDTO> StuLst { get; set; } = new List<AddStudentInfoDTO>();
    }

    

    public class GetSubChapeterReqDTO
    {
        
        public Guid StdId { get; set; }
        public Guid SubId { get; set; }
    }

    public class AddSubChapeterDTO
    {   
        public Guid MapId { get; set; }
        public string ChapName { get; set; }
    }

    public class SubChapeterDTO
    {
        public Guid MapId { get; set; }
        public Guid ChapId { set; get; }
        public string ChapName { get; set; }
    }


    public class StdSubMapDTO
    { 
        public Guid MapId { get; set; }

        public string SubId { get; set; }

        public string StdId { get; set; }

        public string SubName { get; set; }
        public string SubCode { get; set; }
        public int? IsLanguage { get; set; }
        public int? LangOrdinal { get; set; }
    }

    public class StandardDTO
    {   
        public Guid StdId { get; set; }
        public Guid OrgId { get; set; }
        public string StdName { get; set; }
    }

    public class SubjectDTO
    {
        public Guid SubId { get; set; }
        public Guid OrgId { get; set; }
        public string SubjectName { get; set; }
        public string SubCode { get; set; }
        public int? IsLanguage { get; set; }
        public int? LangOrdinal { get; set; }
    }
    public class OrgAdminDTO
    { 
        public Guid MapId { get; set; }
        public Guid OrgId { get; set; }
        public string OrgName { get; set; }
        public Guid AdminUserId { get; set; }
        public string AdmingName { get; set; }
    }
    public class EnquiryDTO
    {
        
        public long ID { get; set; }
        
        public string SchoolName { get; set; }
        
        public string ContactName { get; set; }
        
        public string Phone { get; set; }
        
        public string Email { get; set; }

        
        public DateTime EnqDate { get; set; }

        
        public string Status { get; set; }
    }

    public class UserOrgMapDTO
    {
        
        public Guid MapId { get; set; }
        
        public Guid UserId { get; set; }
        
        public Guid OrgId { get; set; }
        
        public Guid RoleId { get; set; }

        public string OrgName { get; set; }

        public string RoleName { get; set; }

        public int RoleValue { get; set; }
    }


    public class RoleMasterDTO
    {   
        public Guid RoleID { get; set; }
        public int RoleVal { get; set; }
        public string RoleName { get; set; }
    }

    public class UserInfoDTO
    {
        public Guid? Id { get; set; }
        public Guid? OrgId { get; set; }
        public int? RoleVal { get; set; }
        public Guid? MapId { get; set; }
        public string RoleName { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public string password { get; set; }
        public string MobNo { get; set; }
        public string Emailid { get; set; }
    }

    public class OrganizationDTO
    {   
        public Guid? OrgId { get; set; }
        public string OrgCode { get; set; }
        public string OrgName { get; set; }
        
        public string OrgAddress { get; set; }
        
        public string OrgPOC { get; set; }

        
        public string OrgEmail { get; set; }
        
        public string OrgMobile { get; set; }

    }

    public class NotificationSentItemsComparer : IEqualityComparer<NotificationSentTempDTO>
    {
        public bool Equals([AllowNull] NotificationSentTempDTO x, [AllowNull] NotificationSentTempDTO y)
        {
            return x.NID == y.NID;
        }

        public int GetHashCode([DisallowNull] NotificationSentTempDTO obj)
        {
            return obj.NID.GetHashCode();
        }
    }
}
