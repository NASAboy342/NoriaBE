using System;
using NoriaBE.Models;

namespace NoriaBE.Services;

public interface IPaymentService
{
    void CreatePayment(Usage payment);
    void DoPayment(Usage targetPayment, Usage payment);
    List<Usage> GetRoomPayments(int roomId, DateTime startTime, DateTime endTime);
    Usage GetRoomPaymentsById(int id);
    void Update(Usage updatedPayment);
}
