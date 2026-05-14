using System;
using NoriaBE.Models;
using NoriaBE.Repositories;

namespace NoriaBE.Services;

public class PaymentService : IPaymentService
{
    private readonly INoriaRepository _repository;
    public PaymentService(INoriaRepository repository)
    {
        _repository = repository;
    }

    public void CreatePayment(Usage payment)
    {
        _repository.CreatePayment(payment);
    }

    public void DoPayment(Usage targetPayment, Usage payment)
    {
        targetPayment.TotalAmmountPaid = payment.TotalAmmountPaid;
        targetPayment.IsPaid = payment.TotalAmmountPaid >= targetPayment.TotalAmmountToPay;
        targetPayment.PaidOn = DateTime.Now;
    }

    public List<Usage> GetRoomPayments(int roomId, DateTime startTime, DateTime endTime)
    {
        return _repository.GetRoomPayments(roomId, startTime, endTime);
    }

    public Usage GetRoomPaymentsById(int id)
    {
        return _repository.GetRoomPaymentsById(id);
    }

    public void Update(Usage updatedPayment)
    {
        _repository.UpdatePayment(updatedPayment);
    }
}
