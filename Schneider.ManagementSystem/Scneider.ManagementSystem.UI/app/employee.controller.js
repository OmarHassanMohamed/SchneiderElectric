(function () {
    "use strict";
    angular.module("app").controller("EmployeeController", employeeController);
    function employeeController($http) {
        var vm = this;
        var dataService = $http;
        vm.employees = [];
        vm.addClick = addClick;
        vm.cancelClick = cancelClick;
        vm.editClick = editClick;
        vm.deleteClick = deleteClick;
        vm.saveClick = saveClick;
        vm.search = search;
        vm.resetSearch = resetSearch;
        vm.employee = {};
        vm.searchImmediate = searchImmediate;
        //Const For Page Mode(Parts)
        const pageMode = {
            LIST: "List",
            EDIT: "Edit",
            ADD: "Add"
        };
        //Define Each Mode Properties 
        vm.uiState = {
            mode: pageMode.LIST,
            isDetailAreaVisible: false,
            isListAreaVisible: true,
            isSearchAreaVisible: true,
            isValid: true,
            messages: []
        };
        //dynamic change view mode 
        function setUiState(state) {
            vm.uiState.mode = state;
            vm.uiState.isDetailAreaVisible = (state == pageMode.ADD || state == pageMode.EDIT);
            vm.uiState.isListAreaVisible = (state == pageMode.LIST);
            vm.uiState.isSearchAreaVisible = (state == pageMode.LIST);
        }
        //handle add button
        function addClick() {
            vm.employee = initEntity();
            setUiState(pageMode.ADD);
        }
        //handle edit button
        function editClick(id) {
            productGet(id);
            setUiState(pageMode.EDIT);
        }
        //handle cancel button
        function cancelClick() {
            setUiState(pageMode.LIST);
        }
        //handle delete button
        function deleteClick(id) {
            if (confirm("Delete This Employee")) {
                deleteData(id);
            }
        }
        //handle save button
        function saveClick(employeeForm) {
            //    if (validateDate()) {
            if (vm.uiState.mode === pageMode.ADD) {
                insertData();
            } else {
                updateData();
            }
            //} else {
            //    vm.uiState.isValid = false;
            setUiState(pageMode.LIST);
            //}
        }
        function insertData() {
            dataService.post("http://localhost:63157/api/Employee/", vm.employee)
                .then(function (result) {
                   // vm.employee = result.data;
                    vm.employees.push(vm.employee);
                    setUiState(pageMode.LIST);
                }, function (error) {
                    handleException(error);
                });
        }

        function updateData() {
            dataService.put("http://localhost:63157/api/Employee/"+vm.employee.Id, vm.employee)
                .then(function (result) {
                  //  vm.employee = result.data;
                    var index = vm.employees.map(function (e) { return p.Id })
                        .indexOf(vm.employee.Id);
                    vm.employees[index] = vm.employee;
                    setUiState(pageMode.LIST);
                },
                function (error) {
                    handleException(error);
                });
        }

        function deleteData(id) {
            dataService.delete("http://localhost:63157/api/Employee/"+id)
                .then(function (result) {
                    var index = vm.employees.map(function (p) {
                        return p.Id;
                    }).indexOf(id);
                    vm.employees.splice(index, 1);
                    setUiState(pageMode.LIST);
                },
                function (error) {
                    handleException(error);
                });
        }

        function productGet(id) {
            dataService.get("http://localhost:63157/api/Employee/"+id)
                .then(function (result) {
                    vm.employee = result.data;
                    if (vm.employee.JoinDate != null) {
                        vm.employee.JoinDate =
                            new Date(vm.product.JoinDate)
                                .toLocaleDateString();
                    }
                }, function (error) {
                    handleException(error);
                });
        }

        //retrieve Employees from api
        function employeeList()
        {
            dataService.get("http://localhost:63157/api/Employee")
                .then(function (result) {
                    vm.employees = result.data;
                    setUiState(pageMode.LIST);
                },
                function (error) {
                    handleException(error);
                });
        }
        employeeList();//fill employees array

        vm.searchInput = {
            selectedDepartment: {
                Id: 0,
                Name: ""
            },
            Name: ""
        };

        function search() {
            setUiState(pageMode.LIST);
        }
        function resetSearch() {
            vm.searchInput = {
                selectedDepartment: {
                    Id: 0,
                    Name: ""
                },
                Name: ""
            };
        }

        function initEntity() {
            return {
                Name: "",
                Mail: "",
                Department: {
                    Id: 1,
                    Name: ""
                }
            };
        }

        function searchImmediate(item) {
            if ((vm.searchInput.selectedDepartment.Id == 0
                ? true
                : vm.searchInput.selectedDepartment.Id == item.Department.Id) &&
                (vm.searchInput.Name.length == 0
                    ? true
                    : (item.Name.toLowerCase().indexOf(vm.searchInput.Name.toLowerCase()) >= 0))) {
                return true;
            } else {
                return false;
            }
        }

        function validateDate() {
            vm.employee.JoinDate = vm.employee.JoinDate.replace(/\u200E/g, '');
            vm.uiState.messages = [];
            if (vm.employee.JoinDate != null) {
                if (isNaN(Date.parse(vm.employee.JoinDate))) {
                    addValidationMessage("JoinDate", "Invalid Introduction Date");
                }
            }
        }

        function addValidationMessage(prop, msg) {
            vm.uiState.messages.push({
                property: prop,
                messages: msg
            });
        }


        //Error handling 
        function handleException(error) {
            vm.uiState.isValid = false;
            var msg = {
                property: "Error",
                message: ""
            };
            vm.uiState.messages = [];
            switch (error.status) {
                case 400: //Bad Request
                    break;
                case 404: //Not Found
                    msg.message = "The Employee you were requesting could not be found";
                    vm.uiState.messages.push(msg);
                    break;
                case 500: //Internal Error
                    msg.message = error.data.ExceptionMessage;
                    vm.uiState.messages.push(msg);
                    break;
                default:
                    msg.message = "Status:" +
                        error.status +
                        "- Error Message: " +
                        error.statusText;
                    vm.uiState.messages.push(msg);
                    break;
            }
        }

    }
})();