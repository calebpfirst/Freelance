#include <iostream>
#include <string>

using namespace std;

class Business 
{
protected:
    int* maxOccupancy;

public:
    Business() :
        maxOccupancy(nullptr),
        name(""),
        address("")
    {
        this->SetMaxOccupancy(-1);
    }

    Business(const Business& cpyBusiness)
    {
        this->address = cpyBusiness.address;
        this->name = cpyBusiness.name;
        this->SetMaxOccupancy(cpyBusiness.GetMaxOccupancy());
    }

    ~Business()
    {
        delete maxOccupancy;
        maxOccupancy = nullptr;
    }

    Business& operator=(const Business& cpyBusiness)
    {
        this->address = cpyBusiness.address;
        this->name = cpyBusiness.name;
        this->SetMaxOccupancy(cpyBusiness.GetMaxOccupancy());

        return *this;
    }

    string GetName()
    {
        return name;
    }

    void SetName(string busName) 
    {
        name = busName;
    }

    string GetAddress() const
    {
        return address;
    }

    void SetAddress(string busAddress) 
    {
        address = busAddress;
    }

    int GetMaxOccupancy() const
    {
        return (maxOccupancy == nullptr) ? 0 : *maxOccupancy;
    }

    void SetMaxOccupancy(int maxOccupancyTo)
    {
        if (this->maxOccupancy == nullptr)
            maxOccupancy = new int;
        *this->maxOccupancy = maxOccupancyTo;
    }

    string GetName() const
    {
        return name;
    }

    string GetDescription() const 
    {
        return name + " -- " + address;
    }

    void PrintBusiness()
    {
        cout << "name: " << name;
        cout << "\naddress: " << address;
        cout << "\n";
    }

private:
    string name;
    string address;
};

class Restaurant : public Business 
{
public:
    Restaurant() :
        Business(),
        rating(0)
    {
    }

    Restaurant(const Restaurant& cpyRestaurant) :
        Business()
    {
        this->SetRating(cpyRestaurant.GetRating());
        this->SetAddress(cpyRestaurant.GetAddress());
        this->SetName(cpyRestaurant.GetName());
        this->SetMaxOccupancy(cpyRestaurant.GetMaxOccupancy());
    }

    ~Restaurant()
    {
    }

    Restaurant& operator=(const Restaurant& cpyRestaurant)
    {
        this->SetRating(cpyRestaurant.GetRating());
        this->SetAddress(cpyRestaurant.GetAddress());
        this->SetName(cpyRestaurant.GetName());
        this->SetMaxOccupancy(cpyRestaurant.GetMaxOccupancy());

        return *this;
    }

    void SetRating(int userRating) 
    {
        rating = userRating;
    }

    int GetRating() const 
    {
        return rating;
    }

    void PrintRestaurant()
    {
        cout << "rating: " << to_string(rating) << "\n";
    }

    void Print()
    {
        PrintBusiness();
        PrintRestaurant();
    }
private:
    int rating;
};

int main() {
    Business someBusiness;
    Restaurant favoritePlace;

    someBusiness.SetName("ACME");
    someBusiness.SetAddress("4 Main St");

    favoritePlace.SetName("Friends Cafe");
    favoritePlace.SetAddress("500 W 2nd Ave");
    favoritePlace.SetRating(5);

    cout << someBusiness.GetDescription() << endl;
    cout << favoritePlace.GetDescription() << endl;
    cout << "  Rating: " << favoritePlace.GetRating() << endl;

    Business* testBusiness = new Business(someBusiness);
    Restaurant* testRestaurant = new Restaurant(favoritePlace);

    someBusiness = *testBusiness;
    favoritePlace = *testRestaurant;

    testBusiness->PrintBusiness();
    testRestaurant->Print();

    someBusiness.PrintBusiness();
    testRestaurant->Print();

    delete testBusiness;
    testBusiness = nullptr;

    delete testRestaurant;
    testRestaurant = nullptr;

    return 0;
}
