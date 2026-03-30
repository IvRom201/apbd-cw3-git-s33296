using System.Collections.Generic;
using apbd_cw3_s33296.domain;
using apbd_cw3_s33296.repo;

namespace apbd_cw3_s33296.service;

public class UserService
{
    private readonly IUserRepo userRepo;
    private readonly IIdGenerator idGenerator;

    public UserService(IUserRepo userRepo, IIdGenerator idGenerator)
    {
        this.userRepo = userRepo;
        this.idGenerator = idGenerator;
    }
    
    public Student AddStudent(string firstName, string lastName, string studentNumber, string faculty)
    {
        var student = new Student(
            idGenerator.NextUserId(),
            firstName,
            lastName,
            studentNumber,
            faculty);

        userRepo.Add(student);
        return student;
    }

    public Employee AddEmployee(string firstName, string lastName, string department, string position)
    {
        var employee = new Employee(
            idGenerator.NextUserId(),
            firstName,
            lastName,
            department,
            position);

        userRepo.Add(employee);
        return employee;
    }

    public IReadOnlyList<User> GetAllUsers() => userRepo.GetAll();
}