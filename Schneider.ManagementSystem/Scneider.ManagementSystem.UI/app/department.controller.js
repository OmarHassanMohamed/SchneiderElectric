(function () {
    "use strict";
    angular.module("app").controller("DepartmentController", departmentController);

    function departmentController($http) {
        var dvm = this;
        var dataService = $http;
        dvm.departments = [];
        dvm.searchDepartments = [];
        dvm.addClick = addClick;
        dvm.cancelClick = cancelClick;
        dvm.editClick = editClick;
        dvm.deleteClick = deleteClick;
        dvm.saveClick = saveClick;
        dvm.department = {};

        //Const For Page Mode(Parts)
        const pageMode = {
            LIST: "List",
            EDIT: "Edit",
            ADD: "Add"
        };
        //Define Each Mode Properties 
        dvm.uiState = {
            mode: pageMode.LIST,
            isDetailAreaVisible: false,
            isListAreaVisible: true,
            isValid: true,
            messages: []
        };
        //dynamic change view mode 
        function setUiState(state) {
            dvm.uiState.mode = state;
            dvm.uiState.isDetailAreaVisible = (state == pageMode.ADD || state == pageMode.EDIT);
            dvm.uiState.isListAreaVisible = (state == pageMode.LIST);
        }
        //handle add button
        function addClick() {
            vm.department = initEntity();
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
        function saveClick(departmentForm) {
            //    if (validateDate()) {
            if (dvm.uiState.mode === pageMode.ADD) {
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
            dataService.post("http://localhost:63157/api/Department", dvm.department)
                .then(function (result) {
                    dvm.department = result.data;
                    dvm.departments.push(dvm.department);
                    setUiState(pageMode.LIST);
                }, function (error) {
                    handleException(error);
                });
        }

        function updateData() {
            dataService.put("http://localhost:63157/api/Department/"+dvm.department.Id, dvm.department)
                .then(function (result) {
                    dvm.department = result.data;
                    var index = dvm.departments.map(function (e) { return p.Id })
                        .indexOf(dvm.department.id);
                    dvm.departments[index] = dvm.department;
                    setUiState(pageMode.LIST);
                },
                function (error) {
                    handleException(error);
                });
        }

        function deleteData(id) {
            dataService.delete("http://localhost:63157/api/department/"+id)
                .then(function (result) {
                    var index = dvm.departments.map(function (p) {
                        return p.Id;
                    }).indexOf(id);
                    dvm.departments.splice(index, 1);
                    setUiState(pageMode.LIST);
                },
                function (error) {
                    handleException(error);
                });
        }

        function productGet(id) {
            dataService.get("http://localhost:63157/api/Department/"+id)
                .then(function (result) {
                    dvm.department = result.data;
                }, function (error) {
                    handleException(error);
                });
        }

        //retrieve Employees from api
        function employeeList() {
            dataService.get("http://localhost:63157/api/Employee")
                .then(function (result) {
                    dvm.employees = result.data;
                    setUiState(pageMode.LIST);
                },
                function (error) {
                    handleException(error);
                });
        }
        employeeList();//fill employees array

        dvm.searchInput = {
            selectedDepartment: {
                Id: 0,
                Name: ""
            },
            Name: ""
        };

        function initEntity() {
            return {
                Name: "",
                Id: 0
            };
        }

        //function validateDate() {
        //    vm.employee.JoinDate = vm.employee.JoinDate.replace(/\u200E/g, '');
        //    vm.uiState.messages = [];
        //    if (vm.employee.JoinDate != null) {
        //        if (isNaN(Date.parse(vm.employee.JoinDate))) {
        //            addValidationMessage("JoinDate", "Invalid Introduction Date");
        //        }
        //    }
        //}

        function addValidationMessage(prop, msg) {
            vm.uiState.messages.push({
                property: prop,
                messages: msg
            });
        }


        //fill departments array
        function departmentList() {
            dataService.get("http://localhost:63157/api/Department")
                .then(function (result) {
                    dvm.departments = result.data;
                },
                function (error) {
                    handleException(error);
                });
        }


        departmentList();
        searchDepartmentList();
        //fill searchDepartments array
        function searchDepartmentList() {
            dataService.get("http://localhost:63157/api/Department/GetSearchDepartments")
                .then(function (result) {
                    dvm.searchDepartments = result.data;
                },
                function (error) {
                    handleException(error);
                });
        }

        //Error handling 
        function handleException(error) {
            dvm.uiState.isValid = false;
            var msg = {
                property: "Error",
                message: ""
            };
            dvm.uiState.messages = [];
            switch (error.status) {
                case 400: //Bad Request
                    break;
                case 404: //Not Found
                    msg.message = "The Department you were requesting could not be found";
                    dvm.uiState.messages.push(msg);
                    break;
                case 500: //Internal Error
                    msg.message = error.data.ExceptionMessage;
                    dvm.uiState.messages.push(msg);
                    break;
                default:
                    msg.message = "Status:" +
                        error.status +
                        "- Error Message: " +
                        error.statusText;
                    dvm.uiState.messages.push(msg);
                    break;
            }
        }
    }
})();