using DarakhsHC_API.Library.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DarakhsHC_API.Library.SqlAccess.PatientsInfo
{
    public class PatientsInfoAccess
    {
        public static string connString = ConfigurationManager.ConnectionStrings["ConnStringDb"].ConnectionString;


        public static DashboardResponseDto GetDashboardStats(int compId)
        {
            var response = new DashboardResponseDto
            {
                HourlyAppointments = new List<HourlyAppointmentDto>(),
                MonthlyStats = new List<MonthlyStatsDto>(),
                AppointmentVsWalkin = new List<DonutChartDto>(),
                TreatmentsMonthlyStatsDto = new List<TreatmentsMonthlyStatsDto>(),
                TreatmentsDonutChartDto = new List<TreatmentsDonutChartDto>()
            };

            using (SqlConnection con = new SqlConnection(connString))
            using (SqlCommand cmd = new SqlCommand("usp_Dashboard_PatientStats", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyId", SqlDbType.Int).Value = compId;
                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    /* ===============================
                       RESULT SET 1: KPI CARDS
                       =============================== */
                    if (reader.Read())
                    {
                        response.Kpis = new DashboardKpiDto
                        {
                            TodaysAppointmentCount = reader.GetInt32(reader.GetOrdinal("TodaysAppointmentCount")),
                            TodaysPatientCount = reader.GetInt32(reader.GetOrdinal("TodaysPatientCount")),
                            ThisMonthPatientCount = reader.GetInt32(reader.GetOrdinal("ThisMonthPatientCount")),
                            TodaysFollowUpCount = reader.GetInt32(reader.GetOrdinal("TodaysFollowUpCount")),
                            TodaysSummaryCount = reader.GetInt32(reader.GetOrdinal("TodaysSummaryCount"))
                        };
                    }

                    /* ===============================
                       RESULT SET 2: Hour-wise Appointments
                       =============================== */
                    reader.NextResult();
                    while (reader.Read())
                    {
                        response.HourlyAppointments.Add(new HourlyAppointmentDto
                        {
                            HourSlot = reader["HourSlot"].ToString(),
                            PatientCount = Convert.ToInt32(reader["PatientCount"])
                        });
                    }

                    /* ===============================
                       RESULT SET 3: Month-wise Stats
                       =============================== */
                    reader.NextResult();
                    while (reader.Read())
                    {
                        response.MonthlyStats.Add(new MonthlyStatsDto
                        {
                            MonthName = reader["MonthName"].ToString(),
                            MonthNumber = Convert.ToInt32(reader["MonthNumber"]),
                            YearNumber = Convert.ToInt32(reader["YearNumber"]),
                            PatientCount = Convert.ToInt32(reader["PatientCount"]),
                            AppointmentCount = Convert.ToInt32(reader["AppointmentCount"])
                        });
                    }

                    /* ===============================
                       RESULT SET 4: Donut Chart
                       =============================== */
                    reader.NextResult();
                    while (reader.Read())
                    {
                        response.AppointmentVsWalkin.Add(new DonutChartDto
                        {
                            PatientType = reader["PatientType"].ToString(),
                            PatientCount = Convert.ToInt32(reader["PatientCount"])
                        });
                    }

                    /* ===============================
                      RESULT SET 5: Month-wise Hearing & Speaking Count
                      =============================== */
                    reader.NextResult();
                    while (reader.Read())
                    {
                        var tms = new TreatmentsMonthlyStatsDto
                        {
                            MonthName = reader["MonthName"].ToString(),
                            MonthNumber = Convert.ToInt32(reader["MonthNumber"]),
                            YearNumber = Convert.ToInt32(reader["YearNumber"]),
                            TreatmentCounts = new Dictionary<string, int>()
                        };

                        // Loop over all columns except MonthName, MonthNumber, YearNumber
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string colName = reader.GetName(i);
                            if (colName != "MonthName" && colName != "MonthNumber" && colName != "YearNumber")
                            {
                                tms.TreatmentCounts[colName] = Convert.ToInt32(reader[i]);
                            }
                        }

                        response.TreatmentsMonthlyStatsDto.Add(tms);
                    }

                    /* ===============================
                       RESULT SET 6: Donut Chart - Current Month Treatment-wise Count
                       =============================== */
                    reader.NextResult();
                    while (reader.Read())
                    {
                        response.TreatmentsDonutChartDto.Add(new TreatmentsDonutChartDto
                        {
                            TreatmentType = reader["TreatmentType"].ToString(),
                            TreatmentCount = Convert.ToInt32(reader["PatientCount"])
                        });
                    }
                }
            }

            return response;

        }

        public static int UpsertPatientAppointment(PatientsAppointmentInfo appointmentInfo)
        {
            try
            {
                string sql = PatientsInfo.UpsertPatientsAppointment;
                // string strConString = @"Data Source=DESKTOP-8G6MFH8\MSSQLSERVER01;Initial Catalog=DarakhHC;Integrated Security=True";
                string strConString = connString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@Id", appointmentInfo.Id);
                    cmd.Parameters.AddWithValue("@FormDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@MS_Comp_Id", appointmentInfo.MS_Comp_Id);
                    cmd.Parameters.AddWithValue("@PatientsName", appointmentInfo.PatientsName);
                    cmd.Parameters.AddWithValue("@EnquiryFor", !string.IsNullOrEmpty(appointmentInfo.EnquiryFor) ? appointmentInfo.EnquiryFor : "");
                    cmd.Parameters.AddWithValue("@Mobile", appointmentInfo.Mobile > 0 ? appointmentInfo.Mobile : 0);
                    cmd.Parameters.AddWithValue("@AppointmentDate", appointmentInfo.AppointmentDate == DateTime.MinValue ? DateTime.MinValue : appointmentInfo.AppointmentDate);
                    cmd.Parameters.AddWithValue("@MS_Reference_Id", appointmentInfo.MS_Reference_Id > 0 ?
                        appointmentInfo.MS_Reference_Id : 0 );
                    cmd.Parameters.AddWithValue("@Address", !string.IsNullOrEmpty(appointmentInfo.Address) ? appointmentInfo.Address : "");

                    // cmd.ExecuteNonQuery();
                    object returnObj = cmd.ExecuteScalar();

                    if (returnObj != null)
                    {
                        Int32.TryParse(returnObj.ToString(), out int returnValue);
                        return returnValue;
                    }

                    return 0;

                    // var ddd = employeeFamilyInformation.Id;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static bool IsSameDayAppointmentExistsForPatient(PatientsAppointmentInfo patientsAppointment)
        {
            bool exists = false;
            string strConString = connString;
            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = PatientsInfo.CheckSameDayAppointment;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@PatientsName", patientsAppointment.PatientsName);
                    command.Parameters.AddWithValue("@Mobile", patientsAppointment.Mobile);
                    command.Parameters.AddWithValue("@AppointmentDate", patientsAppointment.AppointmentDate);
                    command.Parameters.AddWithValue("@CompId", patientsAppointment.MS_Comp_Id);

                    var result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        exists = Convert.ToBoolean(result);
                    }

                }
            }

            return exists;
        }

        public static List<PatientsAppointmentInfo> GetPatientAppointments(int CompanyId, DateTime fromDt, DateTime toDt)
        {
            List<PatientsAppointmentInfo> appointments = new List<PatientsAppointmentInfo>();
            // string strConString = @"Data Source=DESKTOP-8G6MFH8\MSSQLSERVER01;Initial Catalog=DarakhHC;Integrated Security=True";
            string strConString = connString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = PatientsInfo.GetPatientsAppointments;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@MS_Comp_Id", CompanyId);
                    command.Parameters.AddWithValue("@FromDate", fromDt);
                    command.Parameters.AddWithValue("@ToDate", toDt);


                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var appointment = new PatientsAppointmentInfo
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                FormDate = reader["FormDate"] != DBNull.Value ? Convert.ToDateTime(reader["FormDate"].ToString()) : DateTime.MinValue,
                                MS_Comp_Id = reader["MS_Comp_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Comp_Id"].ToString()) : 0,
                                PatientsName = reader["PatientsName"] != DBNull.Value ? reader["PatientsName"].ToString() : null,
                                EnquiryFor = reader["EnquiryFor"] != DBNull.Value ? reader["EnquiryFor"].ToString() : null,
                                Mobile = reader["Mobile"] != DBNull.Value ? Convert.ToDecimal(reader["Mobile"].ToString()) : 0,
                                AppointmentDate = reader["AppointmentDate"] != DBNull.Value ? Convert.ToDateTime(reader["AppointmentDate"].ToString()) : DateTime.MinValue,
                                MS_Reference_Id = reader["MS_Reference_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Reference_Id"].ToString()) : 0,
                                ReferenceName = reader["ReferenceName"] != DBNull.Value ? reader["ReferenceName"].ToString() : null,
                                Address = reader["Address"] != DBNull.Value ? reader["Address"].ToString() : null,
                            };
                            appointments.Add(appointment);
                        }
                    }
                }
            }

            return appointments;
        }

        public static int UpsertPatientsInfo(Library.Models.PatientsInfo patientsInfo)
        {
            try
            {
                string sql = PatientsInfo.UpsertPatients;
                // string strConString = @"Data Source=DESKTOP-8G6MFH8\MSSQLSERVER01;Initial Catalog=DarakhHC;Integrated Security=True";
                string strConString = connString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@Id", patientsInfo.Id);
                    cmd.Parameters.AddWithValue("@MS_Comp_Id", patientsInfo.MS_Comp_Id);
                    cmd.Parameters.AddWithValue("@Patients_Appointment_Id", patientsInfo.Patients_Appointment_Id);
                    cmd.Parameters.AddWithValue("@FName", !string.IsNullOrEmpty(patientsInfo.FName) ? patientsInfo.FName : "");
                    cmd.Parameters.AddWithValue("@LName", !string.IsNullOrEmpty(patientsInfo.LName) ? patientsInfo.LName : "");
                    cmd.Parameters.AddWithValue("@FullName", patientsInfo.FName + " " + patientsInfo.LName);
                    cmd.Parameters.AddWithValue("@DOB", patientsInfo.DOB == DateTime.MinValue ? DateTime.MaxValue.Date : patientsInfo.DOB.Date);
                    cmd.Parameters.AddWithValue("@Mobile1", patientsInfo.Mobile1 > 0 ? patientsInfo.Mobile1 : 0);
                    cmd.Parameters.AddWithValue("@Mobile2", patientsInfo.Mobile2 > 0 ? patientsInfo.Mobile2 : 0);
                    cmd.Parameters.AddWithValue("@AddressLine1", !string.IsNullOrEmpty(patientsInfo.AddressLine1) ? patientsInfo.AddressLine1 : "");
                    cmd.Parameters.AddWithValue("@AddressLine2", !string.IsNullOrEmpty(patientsInfo.AddressLine2) ? patientsInfo.AddressLine2 : "");
                    cmd.Parameters.AddWithValue("@MS_City_Id", patientsInfo.MS_City_Id != 0 ? patientsInfo.MS_City_Id : 0);
                    cmd.Parameters.AddWithValue("@MS_State_Id", patientsInfo.MS_State_Id != 0 ? patientsInfo.MS_State_Id : 0);
                    cmd.Parameters.AddWithValue("@PostalCode", patientsInfo.PostalCode > 0 ? patientsInfo.PostalCode : 0);
                    cmd.Parameters.AddWithValue("@MaritialStatus", !string.IsNullOrEmpty(patientsInfo.MaritialStatus) ? patientsInfo.MaritialStatus : "");
                    cmd.Parameters.AddWithValue("@Gender", patientsInfo.Gender.HasValue ? (object)patientsInfo.Gender.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@MS_Reference_Id", patientsInfo.MS_Reference_Id);
                    cmd.Parameters.AddWithValue("@MS_Treament_Id", patientsInfo.MS_Treament_Id);
                    cmd.Parameters.AddWithValue("@MS_User_Id", patientsInfo.MS_User_Id);
                   // cmd.Parameters.AddWithValue("@IsAmendment", patientsInfo.IsAmendment);
                   cmd.Parameters.AddWithValue("@Remark", !string.IsNullOrEmpty(patientsInfo.Remark) ? patientsInfo.Remark : "");
                    cmd.Parameters.AddWithValue("@VisitDate", patientsInfo.VisitDate == DateTime.MinValue ? DateTime.Now.Date : patientsInfo.VisitDate.Date);

                    // cmd.ExecuteNonQuery();
                    object returnObj = cmd.ExecuteScalar();

                    if (returnObj != null)
                    {
                        Int32.TryParse(returnObj.ToString(), out int returnValue);
                        return returnValue;
                    }

                    return 0;

                    // var ddd = employeeFamilyInformation.Id;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static bool HasPatienAlreadyExists(Models.PatientsInfo patientsInfo)
        {
            bool exists = false;
            string strConString = connString;
            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = PatientsInfo.HasPatientExists;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@MS_Comp_Id", patientsInfo.MS_Comp_Id);
                    command.Parameters.AddWithValue("@Mobile", patientsInfo.Mobile1);
                    command.Parameters.AddWithValue("@Name", patientsInfo.FullName);

                    var result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        exists = Convert.ToBoolean(result);
                    }

                }
            }

            return exists;
        }

        public static List<Library.Models.PatientsInfo> GetPatientInfo(int CompanyId, DateTime fromDt, DateTime toDt)
        {
            List<Library.Models.PatientsInfo> patientsInfo = new List<Library.Models.PatientsInfo>();
            // string strConString = @"Data Source=DESKTOP-8G6MFH8\MSSQLSERVER01;Initial Catalog=DarakhHC;Integrated Security=True";
            string strConString = connString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = PatientsInfo.GetPatientsInfo;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@MS_Comp_Id", CompanyId);
                    command.Parameters.AddWithValue("@FromDate", fromDt);
                    command.Parameters.AddWithValue("@ToDate", toDt);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            GetPatients(patientsInfo, reader);
                        }
                    }
                }
            }

            return patientsInfo;
        }

        private static void GetPatients(List<Models.PatientsInfo> patientsInfo, SqlDataReader reader)
        {
            var info = new Library.Models.PatientsInfo
            {
                Id = Convert.ToInt32(reader["Id"].ToString()),
                FormDate = reader["FormDate"] != DBNull.Value ? Convert.ToDateTime(reader["FormDate"].ToString()) : DateTime.MinValue,
                MS_Comp_Id = reader["MS_Comp_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Comp_Id"].ToString()) : 0,
                Patients_Appointment_Id = reader["Patients_Appointment_Id"] != DBNull.Value ? Convert.ToInt32(reader["Patients_Appointment_Id"].ToString()) : 0,
                FName = reader["FName"] != DBNull.Value ? reader["FName"].ToString() : null,
                LName = reader["LName"] != DBNull.Value ? reader["LName"].ToString() : null,
                FullName = reader["FullName"] != DBNull.Value ? reader["FullName"].ToString() : null,
                DOB = reader["DOB"] != DBNull.Value ? Convert.ToDateTime(reader["DOB"].ToString()) : DateTime.MinValue,
                Mobile1 = reader["Mobile1"] != DBNull.Value ? Convert.ToDecimal(reader["Mobile1"].ToString()) : 0,
                Mobile2 = reader["Mobile2"] != DBNull.Value ? Convert.ToDecimal(reader["Mobile2"].ToString()) : 0,
                AddressLine1 = reader["AddressLine1"] != DBNull.Value ? reader["AddressLine1"].ToString() : null,
                AddressLine2 = reader["AddressLine2"] != DBNull.Value ? reader["AddressLine2"].ToString() : null,
                MS_City_Id = reader["MS_City_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_City_Id"].ToString()) : 0,
                CityName = reader["CityName"] != DBNull.Value ? reader["CityName"].ToString() : null,
                MS_State_Id = reader["MS_State_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_State_Id"].ToString()) : 0,
                StateName = reader["StateName"] != DBNull.Value ? reader["StateName"].ToString() : null,
                PostalCode = reader["PostalCode"] != DBNull.Value ? Convert.ToDouble(reader["PostalCode"].ToString()) : 0,
                MaritialStatus = reader["MaritialStatus"] != DBNull.Value ? reader["MaritialStatus"].ToString() : null,
                Gender = reader["Gender"] != DBNull.Value ? Convert.ToChar(reader["Gender"].ToString()) : (char?)null,
                MS_Reference_Id = reader["MS_Reference_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Reference_Id"].ToString()) : 0,
                Reference = reader["Reference"] != DBNull.Value ? reader["Reference"].ToString() : null,
                MS_Treament_Id = reader["MS_Treament_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Treament_Id"].ToString()) : 0,
                TreamentName = reader["TreamentName"] != DBNull.Value ? reader["TreamentName"].ToString() : null,
                MS_User_Id = reader["MS_User_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_User_Id"].ToString()) : 0,
                UserFullName = reader["UserFullName"] != DBNull.Value ? reader["UserFullName"].ToString() : null,
                Remark = reader["Remark"] != DBNull.Value ? reader["Remark"].ToString() : null,
                VisitDate = reader["VisitDate"] != DBNull.Value ? Convert.ToDateTime(reader["VisitDate"].ToString()) : DateTime.MinValue
            };
            patientsInfo.Add(info);
        }

        public static int UpsertPatientSummary(PatientsSummaryInfo patientsSummary)
        {
            try
            {
                string sql = PatientsInfo.UpsertPatientSummary;
                // string strConString = @"Data Source=DESKTOP-8G6MFH8\MSSQLSERVER01;Initial Catalog=DarakhHC;Integrated Security=True";
                string strConString = connString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@Id", patientsSummary.Id);
                    cmd.Parameters.AddWithValue("@MS_Comp_Id", patientsSummary.MS_Comp_Id);
                    cmd.Parameters.AddWithValue("@ReceiptNo", !string.IsNullOrEmpty(patientsSummary.ReceiptNo) ? patientsSummary.ReceiptNo : "");
                    cmd.Parameters.AddWithValue("@MS_Patients_Id", patientsSummary.MS_Patients_Id);
                    cmd.Parameters.AddWithValue("@VisitDate", patientsSummary.VisitDate == DateTime.MinValue ? DateTime.Now : patientsSummary.VisitDate);
                    cmd.Parameters.AddWithValue("@Remark", !string.IsNullOrEmpty(patientsSummary.Remark) ? patientsSummary.Remark : "");
                    cmd.Parameters.AddWithValue("@Notes", !string.IsNullOrEmpty(patientsSummary.Notes) ? patientsSummary.Notes : "");
                    cmd.Parameters.Add("@NextVisitDate", SqlDbType.DateTime).Value = patientsSummary.NextVisitDate.HasValue ? patientsSummary.NextVisitDate.Value : (object)DBNull.Value;
                    cmd.Parameters.AddWithValue("@IsFollowUpReq", patientsSummary.IsFollowUpReq);
                    cmd.Parameters.Add("@FollowUpDdate", SqlDbType.DateTime).Value = patientsSummary.FollowUpDate.HasValue ? patientsSummary.FollowUpDate.Value : (object)DBNull.Value;
                    cmd.Parameters.AddWithValue("@Amount", patientsSummary.Amount);

                    // cmd.ExecuteNonQuery();
                    object returnObj = cmd.ExecuteScalar();

                    if (returnObj != null)
                    {
                        Int32.TryParse(returnObj.ToString(), out int returnValue);
                        return returnValue;
                    }

                    return 0;

                    // var ddd = employeeFamilyInformation.Id;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static List<PatientsSummaryInfo> GetPatientsSummaries(int compId, DateTime fromDt, DateTime toDt)
        {
            List<PatientsSummaryInfo> patientsSummaries = new List<PatientsSummaryInfo>();
            // string strConString = @"Data Source=DESKTOP-8G6MFH8\MSSQLSERVER01;Initial Catalog=DarakhHC;Integrated Security=True";
            string strConString = connString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = PatientsInfo.GetPatientsSummaries;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@compId", compId);
                    command.Parameters.AddWithValue("@FromDate", fromDt);
                    command.Parameters.AddWithValue("@ToDate", toDt);


                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var summaryInfo = new PatientsSummaryInfo
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                FormDate = reader["FormDate"] != DBNull.Value ? Convert.ToDateTime(reader["FormDate"].ToString()) : DateTime.MinValue,
                                MS_Comp_Id = reader["MS_Comp_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Comp_Id"].ToString()) : 0,
                                ReceiptNo = reader["ReceiptNo"] != DBNull.Value ? reader["ReceiptNo"].ToString() : null,
                                MS_Patients_Id = reader["MS_Patients_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Patients_Id"].ToString()) : 0,
                                PatientsName = reader["FullName"] != DBNull.Value ? reader["FullName"].ToString() : null,
                                Mobile = reader["Mobile"] != DBNull.Value ? reader["Mobile"].ToString() : null,
                                MS_Treament_Id = reader["MS_Treament_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Treament_Id"].ToString()) : 0,
                                TreamentName = reader["TreamentName"] != DBNull.Value ? reader["TreamentName"].ToString() : null,
                                AddressLine1 = reader["AddressLine1"] != DBNull.Value ? reader["AddressLine1"].ToString() : null,
                                AddressLine2 = reader["TreamentName"] != DBNull.Value ? reader["AddressLine2"].ToString() : null,
                                PostalCode = reader["PostalCode"] != DBNull.Value ? Convert.ToDecimal(reader["PostalCode"].ToString()) : 0,
                                MS_State_Id = reader["MS_State_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_State_Id"].ToString()) : 0,
                                StateName = reader["StateName"] != DBNull.Value ? reader["StateName"].ToString() : null,
                                MS_City_Id = reader["MS_City_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_City_Id"].ToString()) : 0,
                                CityName = reader["CityName"] != DBNull.Value ? reader["CityName"].ToString() : null,
                                VisitDate = reader["VisitDate"] != DBNull.Value ? Convert.ToDateTime(reader["VisitDate"].ToString()) : DateTime.MinValue,
                                Remark = reader["Remark"] != DBNull.Value ? reader["Remark"].ToString() : null,
                                Notes = reader["Notes"] != DBNull.Value ? reader["Notes"].ToString() : null,
                                NextVisitDate = reader["NextVisitDate"] != DBNull.Value ? Convert.ToDateTime(reader["NextVisitDate"].ToString()) : (DateTime?)null,
                                IsFollowUpReq = reader["IsFollowUpReq"] != DBNull.Value ? Convert.ToBoolean(reader["IsFollowUpReq"].ToString()) : false,
                                FollowUpDate = reader["FollowUpDdate"] != DBNull.Value ? Convert.ToDateTime(reader["FollowUpDdate"]).Date : (DateTime?)null,
                                Amount = reader["Amount"] != DBNull.Value ? Convert.ToDecimal(reader["Amount"].ToString()) : 0,
                                Gender = reader["Gender"] != DBNull.Value ? Convert.ToChar(reader["Gender"].ToString()) : (char?)null,
                            };
                            patientsSummaries.Add(summaryInfo);
                        }
                    }
                }
            }

            return patientsSummaries;
        }

        public static List<PatientsSummaryInfo> GetPatientsFollowUpDateByDate(int compId, DateTime date)
        {
            List<PatientsSummaryInfo> patientsSummaries = new List<PatientsSummaryInfo>();
            string strConString = connString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = PatientsInfo.GetPatientsFollowUpListByDate;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@compId", compId);
                    command.Parameters.AddWithValue("@followupdt", date);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var summaryInfo = new PatientsSummaryInfo
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                ReceiptNo = reader["ReceiptNo"] != DBNull.Value ? reader["ReceiptNo"].ToString() : null,
                                MS_Patients_Id = reader["MS_Patients_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Patients_Id"].ToString()) : 0,
                                PatientsName = reader["FullName"] != DBNull.Value ? reader["FullName"].ToString() : null,
                                Mobile = reader["Mobile"] != DBNull.Value ? reader["Mobile"].ToString() : null,
                                MS_Treament_Id = reader["MS_Treament_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Treament_Id"].ToString()) : 0,
                                TreamentName = reader["TreamentName"] != DBNull.Value ? reader["TreamentName"].ToString() : null,
                                AddressLine1 = reader["AddressLine1"] != DBNull.Value ? reader["AddressLine1"].ToString() : null,
                                AddressLine2 = reader["TreamentName"] != DBNull.Value ? reader["AddressLine2"].ToString() : null,
                                PostalCode = reader["PostalCode"] != DBNull.Value ? Convert.ToDecimal(reader["PostalCode"].ToString()) : 0,
                                StateName = reader["StateName"] != DBNull.Value ? reader["StateName"].ToString() : null,
                                CityName = reader["CityName"] != DBNull.Value ? reader["CityName"].ToString() : null,
                                VisitDate = reader["VisitDate"] != DBNull.Value ? Convert.ToDateTime(reader["VisitDate"].ToString()) : DateTime.MinValue,
                                Remark = reader["Remark"] != DBNull.Value ? reader["Remark"].ToString() : null,
                                Notes = reader["Notes"] != DBNull.Value ? reader["Notes"].ToString() : null,
                                NextVisitDate = reader["NextVisitDate"] != DBNull.Value ? Convert.ToDateTime(reader["NextVisitDate"].ToString()) : (DateTime?)null,
                                MS_Reference_Id = reader["MS_Reference_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Reference_Id"].ToString()) : 0,
                                Reference = reader["Reference"] != DBNull.Value ? reader["Reference"].ToString() : null,
                                FollowUpDate = reader["FollowUpDdate"] != DBNull.Value ? Convert.ToDateTime(reader["FollowUpDdate"]).Date : (DateTime?)null,
                                Amount = reader["Amount"] != DBNull.Value ? Convert.ToDecimal(reader["Amount"].ToString()) : 0,
                            };
                            patientsSummaries.Add(summaryInfo);
                        }
                    }
                }
            }

            return patientsSummaries;
        }

        public static List<TreatmentsInfo> GetTreatments(int CompanyId)
        {
            List<TreatmentsInfo> treatments = new List<TreatmentsInfo>();
            // string strConString = @"Data Source=DESKTOP-8G6MFH8\MSSQLSERVER01;Initial Catalog=DarakhHC;Integrated Security=True";
            string strConString = connString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = PatientsInfo.GetTreatments;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@MS_Comp_Id", CompanyId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var treatment = new TreatmentsInfo
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                MS_Comp_Id = reader["MS_Comp_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Comp_Id"].ToString()) : 0,
                                TreamentName = reader["TreamentName"] != DBNull.Value ? reader["TreamentName"].ToString() : null
                            };
                            treatments.Add(treatment);
                        }
                    }
                }
            }

            return treatments;
        }

        public static List<ReferencesInfo> GetReferences(int CompanyId)
        {
            List<ReferencesInfo> references = new List<ReferencesInfo>();
            // string strConString = @"Data Source=DESKTOP-8G6MFH8\MSSQLSERVER01;Initial Catalog=DarakhHC;Integrated Security=True";
            string strConString = connString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = PatientsInfo.GetReferences;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@MS_Comp_Id", CompanyId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var reference = new ReferencesInfo
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                MS_Comp_Id = reader["MS_Comp_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Comp_Id"].ToString()) : 0,
                                Reference = reader["Reference"] != DBNull.Value ? reader["Reference"].ToString() : null
                            };
                            references.Add(reference);
                        }
                    }
                }
            }

            return references;
        }

        public static List<StateInfo> GetStates()
        {
            List<StateInfo> states = new List<StateInfo>();
            // string strConString = @"Data Source=DESKTOP-8G6MFH8\MSSQLSERVER01;Initial Catalog=DarakhHC;Integrated Security=True";
            string strConString = connString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = PatientsInfo.GetStates;
                using (var command = new SqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var state = new StateInfo
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                StateCode = reader["StateCode"] != DBNull.Value ? reader["StateCode"].ToString() : null,
                                StateName = reader["StateName"] != DBNull.Value ? reader["StateName"].ToString() : null,
                                MS_Country_Id = reader["MS_Country_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Country_Id"].ToString()) : 0
                            };
                            states.Add(state);
                        }
                    }
                }
            }

            return states;
        }

        public static List<CityInfo> GetCitiesByStateId(int StateId)
        {
            List<CityInfo> cities = new List<CityInfo>();
            // string strConString = @"Data Source=DESKTOP-8G6MFH8\MSSQLSERVER01;Initial Catalog=DarakhHC;Integrated Security=True";
            string strConString = connString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = PatientsInfo.GetCitiesByStateId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@MS_State_Id", StateId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var city = new CityInfo
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                CityName = reader["CityName"] != DBNull.Value ? reader["CityName"].ToString() : null,
                                CityCode = reader["CityCode"] != DBNull.Value ? reader["CityCode"].ToString() : null,
                                MS_State_Id = reader["MS_State_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_State_Id"].ToString()) : 0,
                                StateName = reader["StateName"] != DBNull.Value ? reader["StateName"].ToString() : null
                            };
                            cities.Add(city);
                        }
                    }
                }
            }

            return cities;
        }

        public static List<Models.PatientsInfo> GetPatientsInfoByDate(int CompanyId, DateTime filterdate)
        {
            List<Library.Models.PatientsInfo> patientsInfo = new List<Library.Models.PatientsInfo>();
            // string strConString = @"Data Source=DESKTOP-8G6MFH8\MSSQLSERVER01;Initial Catalog=DarakhHC;Integrated Security=True";
            string strConString = connString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = PatientsInfo.GetPatientsInfoByDate;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@MS_Comp_Id", CompanyId);
                    command.Parameters.AddWithValue("@currentDate", filterdate);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var info = new Library.Models.PatientsInfo
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                FormDate = reader["FormDate"] != DBNull.Value ? Convert.ToDateTime(reader["FormDate"].ToString()) : DateTime.MinValue,
                                MS_Comp_Id = reader["MS_Comp_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Comp_Id"].ToString()) : 0,
                                Patients_Appointment_Id = reader["Patients_Appointment_Id"] != DBNull.Value ? Convert.ToInt32(reader["Patients_Appointment_Id"].ToString()) : 0,
                                FName = reader["FName"] != DBNull.Value ? reader["FName"].ToString() : null,
                                LName = reader["LName"] != DBNull.Value ? reader["LName"].ToString() : null,
                                FullName = reader["FullName"] != DBNull.Value ? reader["FullName"].ToString() : null,
                                DOB = reader["DOB"] != DBNull.Value ? Convert.ToDateTime(reader["DOB"].ToString()) : DateTime.MinValue,
                                Mobile1 = reader["Mobile1"] != DBNull.Value ? Convert.ToDecimal(reader["Mobile1"].ToString()) : 0,
                                Mobile2 = reader["Mobile2"] != DBNull.Value ? Convert.ToDecimal(reader["Mobile2"].ToString()) : 0,
                                AddressLine1 = reader["AddressLine1"] != DBNull.Value ? reader["AddressLine1"].ToString() : null,
                                AddressLine2 = reader["AddressLine2"] != DBNull.Value ? reader["AddressLine2"].ToString() : null,
                                MS_City_Id = reader["MS_City_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_City_Id"].ToString()) : 0,
                                CityName = reader["CityName"] != DBNull.Value ? reader["CityName"].ToString() : null,
                                MS_State_Id = reader["MS_State_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_State_Id"].ToString()) : 0,
                                StateName = reader["StateName"] != DBNull.Value ? reader["StateName"].ToString() : null,
                                PostalCode = reader["PostalCode"] != DBNull.Value ? Convert.ToDouble(reader["PostalCode"].ToString()) : 0,
                                MaritialStatus = reader["MaritialStatus"] != DBNull.Value ? reader["MaritialStatus"].ToString() : null,
                                MS_Reference_Id = reader["MS_Reference_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Reference_Id"].ToString()) : 0,
                                Reference = reader["Reference"] != DBNull.Value ? reader["Reference"].ToString() : null,
                                MS_Treament_Id = reader["MS_Treament_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Treament_Id"].ToString()) : 0,
                                TreamentName = reader["TreamentName"] != DBNull.Value ? reader["TreamentName"].ToString() : null,
                                MS_User_Id = reader["MS_User_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_User_Id"].ToString()) : 0,
                                UserFullName = reader["UserFullName"] != DBNull.Value ? reader["UserFullName"].ToString() : null,
                                IsSummaryGenerated = reader["IsSummaryGenerated"] != DBNull.Value ? reader["IsSummaryGenerated"].ToString() : null,
                                Gender = reader["Gender"] != DBNull.Value ? Convert.ToChar(reader["Gender"].ToString()) : (char?)null,
                            };
                            patientsInfo.Add(info);
                        }
                    }
                }
            }

            return patientsInfo;
        }

       

        public static List<PatientHistory> GetPatientHistoriesById(int compId, int patientId)
        {
            List<PatientHistory> patientHistories = new List<PatientHistory>();
            // string strConString = @"Data Source=DESKTOP-8G6MFH8\MSSQLSERVER01;Initial Catalog=DarakhHC;Integrated Security=True";
            string strConString = connString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = PatientsInfo.GetPatientsHistoryById;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@MS_Comp_Id", compId);
                    command.Parameters.AddWithValue("@MS_Patient_Id", patientId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var info = new PatientHistory
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                FormDate = reader["FormDate"] != DBNull.Value ? Convert.ToDateTime(reader["FormDate"].ToString()) : DateTime.MinValue,
                                MS_Comp_Id = reader["MS_Comp_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Comp_Id"].ToString()) : 0,
                                ReceiptNo = reader["ReceiptNo"] != DBNull.Value ? reader["ReceiptNo"].ToString() : null,
                                MS_Patients_Id = reader["MS_Patients_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Patients_Id"].ToString()) : 0,
                                PatientsName = reader["PatientsName"] != DBNull.Value ? reader["PatientsName"].ToString() : null,
                                Mobile = reader["Mobile"] != DBNull.Value ? Convert.ToDecimal(reader["Mobile"].ToString()) : 0,
                                Address = reader["Address"] != DBNull.Value ? reader["Address"].ToString() : null,
                                PostalCode = reader["PostalCode"] != DBNull.Value ? Convert.ToDouble(reader["PostalCode"].ToString()) : 0,
                                Remark = reader["Remark"] != DBNull.Value ? reader["Remark"].ToString() : null,
                                VisitDate = reader["VisitDate"] != DBNull.Value ? Convert.ToDateTime(reader["VisitDate"].ToString()) : DateTime.MinValue,
                                Note = reader["Notes"] != DBNull.Value ? reader["Notes"].ToString() : null,
                                NextVisitDate = reader["NextVisitDate"] != DBNull.Value ? Convert.ToDateTime(reader["NextVisitDate"].ToString()) : DateTime.MinValue,
                                Amount = reader["Amount"] != DBNull.Value ? Convert.ToDecimal(reader["Amount"].ToString()) : 0,
                                Gender = reader["Gender"] != DBNull.Value ? Convert.ToChar(reader["Gender"].ToString()) : (char?)null,
                            };
                            patientHistories.Add(info);
                        }
                    }
                }
            }

            return patientHistories;
        }


      
    
        public static List<Models.PatientsInfo> GetExistingPatientsInfoByValue(int CompanyId, string value)
        {
            List<Library.Models.PatientsInfo> patientsInfo = new List<Library.Models.PatientsInfo>();
            // string strConString = @"Data Source=DESKTOP-8G6MFH8\MSSQLSERVER01;Initial Catalog=DarakhHC;Integrated Security=True";
            string strConString = connString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = PatientsInfo.GetExistingPatientsInfoByValue;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@MS_Comp_Id", CompanyId);
                    command.Parameters.AddWithValue("@value", value);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            GetPatients(patientsInfo, reader);
                        }
                    }
                }
            }

            return patientsInfo;
        }

        public static void CreatePatientHistory(int compId, int patientId, int patientSummaryId)
        {
            try
            {
                string sql = PatientsInfo.InsertPatientHistory;
                string strConString = connString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@compId", compId);
                    cmd.Parameters.AddWithValue("@patientId", patientId);
                    cmd.Parameters.AddWithValue("@patientSummaryId", patientSummaryId);

                    object returnObj = cmd.ExecuteScalar();

                }
            }
            catch (Exception ex)
            {
            }
        }

        public static int UpsertPatientEnquiry(PatientEnquiry patientEnquiry)
        {
            try
            {
                string sql = PatientsInfo.UpsertPatientEnquiry;
                string strConString = connString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@Id", patientEnquiry.Id);
                    cmd.Parameters.AddWithValue("@MS_Comp_Id", patientEnquiry.MS_Comp_Id);
                    cmd.Parameters.AddWithValue("@Name", patientEnquiry.Name);
                    cmd.Parameters.AddWithValue("@EnquiryFor", !string.IsNullOrEmpty(patientEnquiry.EnquiryFor) ? patientEnquiry.EnquiryFor : "");
                    cmd.Parameters.AddWithValue("@Mobile", patientEnquiry.Mobile > 0 ? patientEnquiry.Mobile : 0);
                    cmd.Parameters.AddWithValue("@MS_Country_Id", patientEnquiry.MS_Country_Id > 0 ? patientEnquiry.MS_Country_Id : 0);
                    cmd.Parameters.AddWithValue("@MS_State_Id", patientEnquiry.MS_State_Id > 0 ? patientEnquiry.MS_State_Id : 0);
                    cmd.Parameters.AddWithValue("@MS_City_Id", patientEnquiry.MS_City_Id > 0 ? patientEnquiry.MS_City_Id : 0);
                    cmd.Parameters.AddWithValue("@Address", !string.IsNullOrEmpty(patientEnquiry.Address) ? patientEnquiry.Address : "");

                    // cmd.ExecuteNonQuery();
                    object returnObj = cmd.ExecuteScalar();

                    if (returnObj != null)
                    {
                        Int32.TryParse(returnObj.ToString(), out int returnValue);
                        return returnValue;
                    }
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static List<PatientEnquiry> GetPatientEnquiriesByDate(int compId, DateTime fromEnquiryDt, DateTime toEnquiryDt)
        {
            List<PatientEnquiry> patientEnquiries = new List<PatientEnquiry>();
            string strConString = connString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = PatientsInfo.GetPatientEquiriesByDate;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@MS_Comp_Id", compId);
                    command.Parameters.AddWithValue("@FromDate", fromEnquiryDt.Date);
                    command.Parameters.AddWithValue("@ToDate", toEnquiryDt.Date);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var patientEnquiry= new PatientEnquiry
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                FormDate = reader["FormDate"] != DBNull.Value ? Convert.ToDateTime(reader["FormDate"].ToString()) : DateTime.MinValue,
                                MS_Comp_Id = reader["MS_Comp_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Comp_Id"].ToString()) : 0,
                                Name = reader["Name"] != DBNull.Value ? reader["Name"].ToString() : null,
                                Mobile = reader["Mobile"] != DBNull.Value ? Convert.ToDecimal(reader["Mobile"].ToString()) : 0,
                                EnquiryFor = reader["EnquiryFor"] != DBNull.Value ? reader["EnquiryFor"].ToString() : string.Empty,
                                Address = reader["Address"] != DBNull.Value ? reader["Address"].ToString() : null,
                                MS_Country_Id = reader["MS_Country_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Country_Id"].ToString()) : 0,
                                CountryName = reader["CountryName"] != DBNull.Value ? reader["CountryName"].ToString() : string.Empty,
                                MS_State_Id = reader["MS_State_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_State_Id"].ToString()) : 0,
                                StateName = reader["StateName"] != DBNull.Value ? reader["StateName"].ToString() : string.Empty,
                                MS_City_Id = reader["MS_City_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_City_Id"].ToString()) : 0,
                                CityName = reader["CityName"] != DBNull.Value ? reader["CityName"].ToString() : string.Empty
                            };
                            patientEnquiries.Add(patientEnquiry);
                        }
                    }
                }
            }

            return patientEnquiries;
        }
    }
}