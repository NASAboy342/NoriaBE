using System;
using NoriaBE.Models;

namespace NoriaBE.Repositories;

public interface INoriaRepository
{
    List<Building> GetAllBuilding();
}
