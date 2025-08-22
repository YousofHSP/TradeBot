using Domain.Entities;

namespace Shared.DTOs;

public class DepositUserDto
{
    
}

public class DepositUserResDto: BaseDto<DepositResDto, Deposit>
{
    public string UserFullName { get; set; }
    public decimal StartAmount { get; set; }
    public decimal EndAmount { get; set; }
}